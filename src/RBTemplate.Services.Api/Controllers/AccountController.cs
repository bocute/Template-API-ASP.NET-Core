using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RBTemplate.Domain.Interfaces;
using RBTemplate.Infra.CrossCutting.Identity.Models;
using RBTemplate.Infra.CrossCutting.Identity.Authorization;
using RBTemplate.Infra.CrossCutting.Identity.Models.AccountVIewModel;
using System.Security.Cryptography;

namespace RBTemplate.Services.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : BaseController
    {
        #region Property
        private ExternalAuthFacebookOptions Options { get; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenDescriptor _tokenDescriptor;
        private readonly INotificationHandler _notifications;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        private readonly IHostEnvironment _hostingEnviroment;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IRefreshTokenRepository<RefreshTokenData> _refreshTokenRepository;

        private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        #endregion

        #region Constructor
        public AccountController(
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    TokenDescriptor tokenDescriptor,
                    INotificationHandler notifications,
                    IUser user,
                    IEmailSender emailSender,
                    IHostEnvironment hostingEnviroment,
                    IConfiguration config,
                    IMapper mapper,
                    HttpClient httpClient,
                    IOptions<ExternalAuthFacebookOptions> optionsAccessor,
                    IRefreshTokenRepository<RefreshTokenData> refreshTokenRepository) : base(notifications, user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenDescriptor = tokenDescriptor;
            _notifications = notifications;
            _emailSender = emailSender;
            _hostingEnviroment = hostingEnviroment;
            _config = config;
            _mapper = mapper;
            _httpClient = httpClient;
            Options = optionsAccessor.Value;
            _refreshTokenRepository = refreshTokenRepository;
        }
        #endregion

        #region Usuario
        [HttpPost]
        [AllowAnonymous]
        [Route("registeruser")]
        public async Task<IActionResult> Register([FromBody] RegisterUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await NotifyErrorModelInvalid();
                return await ResponseAsync();
            }

            var user = new ApplicationUser { Nome = model.Nome, UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Senha);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, new Claim("Global", "Usuario"));

                if (!await IsValidAsync())
                {
                    await _userManager.DeleteAsync(user);
                    return await ResponseAsync(model);
                }

                return await ResponseAsync();
            }

            foreach (var error in result.Errors)
            {
                await NotifyError(error.ToString(), error.Description);
            }

            return await ResponseAsync(model);
        }
        #endregion

        #region Email
        [HttpPost]
        [AllowAnonymous]
        [Route("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await NotifyErrorModelInvalid();
                return await ResponseAsync();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                await NotifyError("ConfirmEmail", "Erro ao confirmar e-mail");
                return await ResponseAsync(model);
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Code);

            if (result.Succeeded)
            {
                var response = new
                {
                    email = user.Email,
                    msg = "E-mail confirmado com sucesso"
                };

                return await ResponseAsync(response);
            }
            else
            {
                await NotifyError("ConfirmEmail", "Erro ao confirmar e-mail");
            }

            return await ResponseAsync(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("sendconfirmemail")]
        public async Task<IActionResult> SendConfirmEmail([FromBody] SendEmailConfirmViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await NotifyErrorModelInvalid();
                return await ResponseAsync();
            }

            if (model.Url != null && _hostingEnviroment.IsProduction())
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    await NotifyError("ConfirmEmail", "Erro ao enviar o e-mail de confirmação.");
                    return await ResponseAsync(model);
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                await SendConfirmEmail(model.Url, model.Email, code);
            }

            return await ResponseAsync();
        }

        private async Task SendConfirmEmail(string url, string email, string code)
        {
            var callbackUrl = url + email + "/" + HttpUtility.UrlEncode(code);

            await _emailSender.SendEmailAsync(email, "Confirmação de E-mail",
                $"Confirme sua e-mail <a href='{callbackUrl}'>clicando aqui</a>.");
        }
        #endregion

        #region Login
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await NotifyErrorModelInvalid();
                return await ResponseAsync(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
            {
                await NotifyError("ConfirmEmail", "E-mail não confirmado");
                return await ResponseAsync(model);
            }

            if (model.GrantType == "password")
            {
                if (model.Senha == null)
                {
                    await NotifyError("Login", "E-mail ou senha incorreto(s)");
                    return await ResponseAsync(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, false, true);

                if (result.Succeeded)
                {
                    var response = await GenerateToken(model);
                    return await ResponseAsync(response);
                }

                await NotifyError(result.ToString(), "E-mail ou senha incorreto(s)");
                return await ResponseAsync(model);
            }
            else if (model.GrantType == "refresh_token")
            {
                if (user == null)
                {
                    await NotifyError("RefreshToken", "Usuário não encontrado");
                    return await ResponseAsync(model);
                }

                var refreshToken = _refreshTokenRepository.GetByRefreshToken(user.Id, model.RefreshToken);

                if (refreshToken == null || refreshToken.ExpirationDate < DateTime.Now)
                {
                    await NotifyError("RefreshToken", "RefreshToken não autorizado");
                    return await ResponseAsync(model);
                }

                var response = await GenerateToken(model);
                return await ResponseAsync(response);
               
            }

            await NotifyError("Login", "Tipo de login inválido.");
            return await ResponseAsync(model);

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("externalauth/facebook")]
        public async Task<IActionResult> LoginFacebook([FromBody] LoginFacebookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await NotifyErrorModelInvalid();
                return await ResponseAsync(model);
            }

            //Remover log
            var serializedData = JsonConvert.SerializeObject(model);

            // 1.generate an app access token
            var appAccessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={Options.IdAppFacebook}&client_secret={Options.SecretKeyAppFacebook}&grant_type=client_credentials");
            var appAccessToken = JsonConvert.DeserializeObject<ExternalAuthFacebook>(appAccessTokenResponse);

            // 2. validate the user access token
            var userAccessTokenValidationResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={model.AccessToken}&access_token={appAccessToken.AccessToken}");
            var userAccessTokenValidation = JsonConvert.DeserializeObject<ExternalAuthFacebookTokenValidation>(userAccessTokenValidationResponse);

            //Remover log
            var serializedData2 = JsonConvert.SerializeObject(userAccessTokenValidation);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                await NotifyError("LoginFacebook", "Erro ao efetuar login com Facebook.");
                return await ResponseAsync(model);
            }

            // 3. we've got a valid token so we can request user data from fb
            //var userInfoResponse = await Client.GetStringAsync($"https://graph.facebook.com/v2.8/me?fields=id,email,first_name,last_name,name,gender,locale,birthday,picture&access_token={model.AccessToken}");
            //var userInfo = JsonConvert.DeserializeObject<FacebookUserData>(userInfoResponse);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                user = new ApplicationUser { Nome = model.Nome, UserName = model.Email, Email = model.Email, FacebookId = model.FacebookId, PictureUrl = model.PictureUrl };

                var resultCreateUser = await _userManager.CreateAsync(user, "X0a@" + Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8));

                if (!resultCreateUser.Succeeded)
                {
                    await NotifyError("LoginFacebook", "Erro ao efetuar login com Facebook..");
                    await _userManager.DeleteAsync(user);
                    return await ResponseAsync(model);
                }
            }

            user = await _userManager.FindByEmailAsync(model.Email);

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                var resultConfirmEmail = await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
                if (resultConfirmEmail.Succeeded)
                {
                    LoginViewModel loginViewModel = new LoginViewModel();
                    loginViewModel.Email = model.Email;
                    var response = await GenerateToken(loginViewModel);
                    return await ResponseAsync(response);
                }
                else
                {
                    await NotifyError("LoginFacebook", "Erro ao efetuar login com Facebook...");
                    await _userManager.DeleteAsync(user);
                    return await ResponseAsync(model);
                }
            }
            else
            {
                LoginViewModel loginViewModel = new LoginViewModel();
                loginViewModel.Email = model.Email;
                var response = await GenerateToken(loginViewModel);
                return await ResponseAsync(response);
            }
        }
        #endregion

        #region Token/Password
        private async Task<object> GenerateToken(LoginViewModel login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            var userClaims = await _userManager.GetClaimsAsync(user);

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            // Necess�rio converver para IdentityClaims
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(userClaims);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenDescriptor.Issuer,
                Audience = _tokenDescriptor.Audience,
                SigningCredentials = SigningCredentialsConfiguration.getSignCredentials(_config),
                Subject = identityClaims,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid)
            });

            var encodedJwt = handler.WriteToken(securityToken);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesValid),
                refresh_token = GenerateRefreshToken(user.Id).RefreshToken,
                user = new
                {
                    id = user.Id,
                    claims = userClaims.Select(c => new { c.Type, c.Value }),
                    pictureUrl = user.PictureUrl
                }
            };

            return response;
        }

        private RefreshTokenData GenerateRefreshToken(string userId)
        {
            string token;
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                token = Convert.ToBase64String(randomNumber);
            }

            var refreshToken = new RefreshTokenData
            {
                UsuarioId = userId,
                ExpirationDate = DateTime.Now.AddMinutes(_tokenDescriptor.MinutesRefreshTokenValid),
                RefreshToken = token.Replace("+", string.Empty)
                                    .Replace("=", string.Empty)
                                    .Replace("/", string.Empty)
            };

            _refreshTokenRepository.Add(refreshToken);

            return refreshToken;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await NotifyErrorModelInvalid();
                return await ResponseAsync(model);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                await NotifyError("ForgotPassword", "O e-mail deve ser confirmado para redefinir a senha.");
                return await ResponseAsync(model);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (model.Url != null && _hostingEnviroment.IsProduction())
                await SendConfirmEmail(model.Url, model.Email, code);

            return await ResponseAsync();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await NotifyErrorModelInvalid();
                return await ResponseAsync(model);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                await NotifyError("ResetPassword", "Erro ao redefinir a senha. Tente novamente.");
                return await ResponseAsync(model);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Senha);
            if (result.Succeeded)
            {
                var response = new
                {
                    email = user.Email,
                    msg = "Senha redefinida com sucesso."
                };

                return await ResponseAsync(response);
            }

            await NotifyError("ResetPassword", "Erro ao redefinir a senha. Tente novamente.");
            return await ResponseAsync(model);

        }

        [HttpPost]
        [Authorize]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await NotifyErrorModelInvalid();
                return await ResponseAsync(model);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null || user.Id != UserId.ToString())
            {
                await NotifyError("ResetPassword", "Erro ao redefinir a senha. Tente novamente.");
                return await ResponseAsync(model);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, code, model.Senha);

            if (result.Succeeded)
            {
                var response = new
                {
                    email = user.Email,
                    msg = "Senha redefinida com sucesso."
                };

                return await ResponseAsync(response);
            }

            await NotifyError("ResetPassword", "Erro ao redefinir a senha. Tente novamente.");
            return await ResponseAsync(model);

        }
        #endregion

    }

}
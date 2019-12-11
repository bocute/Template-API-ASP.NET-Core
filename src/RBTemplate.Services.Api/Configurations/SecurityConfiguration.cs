using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.Configuration;
using Ymagi.Infra.CrossCutting.Identity.Authorization;
using RBTemplate.Infra.CrossCutting.Identity.Models;
using Microsoft.AspNetCore.Identity;
using RBTemplate.Infra.CrossCutting.Identity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using RBTemplate.Infra.CrossCutting.Identity.SendEmail;
using Microsoft.IdentityModel.Tokens;

namespace RBTemplate.Services.Api.Configurations
{
    public static class SecurityConfiguration
    {
        public static void AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var tokenConfigurations = new TokenDescriptor();
            new ConfigureFromConfigurationOptions<TokenDescriptor>(
                    configuration.GetSection("JwtTokenOptions"))
                .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.Lockout.AllowedForNewUsers = false;
                config.Password.RequireDigit = false;
                config.Password.RequiredLength = 8;
                config.Password.RequiredUniqueChars = 0;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.Configure<AuthMessageSenderOptions>(configuration);
            services.Configure<ExternalAuthFacebookOptions>(configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;

                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = SigningCredentialsConfiguration.createKey(configuration),
                    ValidAudience = tokenConfigurations.Audience,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Global", "Admin"));
                options.AddPolicy("Usuario", policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == "Global" && (c.Value == "Usuario" || c.Value == "Admin"))));

                options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ymagi.Infra.CrossCutting.Identity.Authorization
{
    public static class SigningCredentialsConfiguration
    {
        public static SymmetricSecurityKey createKey(IConfiguration config)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["SecretKeyJwT"]));
        }

        public static SigningCredentials getSignCredentials(IConfiguration config)
        {
            return new SigningCredentials(createKey(config), SecurityAlgorithms.HmacSha256Signature);
        }
    }
}

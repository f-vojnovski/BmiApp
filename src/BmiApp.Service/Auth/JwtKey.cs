using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BmiApp.Service.Auth
{
    /// <summary>
    /// Resolves the JWT signing key from configuration. The key is supplied outside of
    /// appsettings.json, through user secrets or an environment variable; see the README.
    /// </summary>
    internal static class JwtKey
    {
        // HMAC-SHA256 expects a key of at least the hash size.
        private const int MinimumKeyBytes = 32;

        public static SymmetricSecurityKey Resolve(IConfiguration configuration)
        {
            var key = configuration.GetSection("Jwt").GetSection("Key").Value;

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException(
                    "The JWT signing key is missing. Provide it with " +
                    "'dotnet user-secrets set \"Jwt:Key\" \"<key>\"' or by setting the " +
                    "Jwt__Key environment variable. See the README for details.");
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);

            if (keyBytes.Length < MinimumKeyBytes)
            {
                throw new InvalidOperationException(
                    $"The JWT signing key must be at least {MinimumKeyBytes} bytes to sign with " +
                    $"HMAC-SHA256; the configured key is {keyBytes.Length} bytes.");
            }

            return new SymmetricSecurityKey(keyBytes);
        }
    }
}

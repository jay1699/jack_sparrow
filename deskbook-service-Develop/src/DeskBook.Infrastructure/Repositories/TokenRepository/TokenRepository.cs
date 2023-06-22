using DeskBook.Infrastructure.Contracts.ITokenRepository;
using DeskBook.Infrastructure.Model.AuthoritySetting;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;

namespace DeskBook.Infrastructure.Repositories.TokenRepository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AuthorityModel _authoritySettings;

        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(AuthorityModel authoritySettings, ILogger<TokenRepository> logger)
        {
            _authoritySettings = authoritySettings;
            _logger = logger;
        }

        public async Task<string> GenerateTokenAsync()
        {
            string tokenUrl = _authoritySettings.TokenUrl;
            string clientId = _authoritySettings.ClientId;
            string secret = _authoritySettings.Secret;
            var tokenClient = new TokenClient(tokenUrl, clientId, secret);
            try
            {
                _logger.LogInformation("Token generated Successfully");
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync("1AuthorityApi");
                string token = string.Format("Bearer {0}", tokenResponse?.AccessToken);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError("Token Generation failed");
                throw ex;
            }
        }
    }
}

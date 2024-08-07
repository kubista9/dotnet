using AzureAD.Configs;
using AzureAD.Models;
using AzureAD.Policies;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace AzureAD.Services
{
	public class Auth0Service : IAuth0Service
	{
		private readonly AzureAdOptions _options;

		public Auth0Service(IOptions<AzureAdOptions> options)
		{
			_options = options.Value;
		}

		public async Task<Token> GetTokenFromAzureAd()
		{
			return await PollyPolicies.RetryPolicy.ExecuteAsync(async () =>
			{
				IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(_options.ClientId)
					.WithClientSecret(_options.ClientSecret)
					.WithAuthority(new Uri(_options.Authority))
					.Build();

				var scopes = new string[] { _options.Scope };

				AuthenticationResult result = await app.AcquireTokenForClient(scopes)
					.ExecuteAsync();

				return new Token
				{
					AccessToken = result.AccessToken,
					ExpiresIn = result.ExpiresOn.UtcDateTime.Subtract(DateTime.UtcNow).TotalSeconds
				};
			});
		}
	}
}

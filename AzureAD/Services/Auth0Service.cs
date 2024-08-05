using System;
using System.Threading.Tasks;
using AzureAD.Configs;
using AzureAD.Models;
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

		public async Task<Token> GetTokenAsync()
		{
			try
			{
				IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(_options.ClientId)
					.WithClientSecret(_options.ClientSecret)
					.WithAuthority(new Uri(_authority))
					.Build();

				var scopes = new string[] { _options.Scope };

				AuthenticationResult result = await app.AcquireTokenForClient(scopes)
					.ExecuteAsync();

				return new Token
				{
					AccessToken = result.AccessToken,
					ExpiresIn = result.ExpiresOn.UtcDateTime.Subtract(DateTime.UtcNow).TotalSeconds
				};
			}
			catch (Exception ex)
			{
				// Handle exceptions
				throw new Exception("Failed to acquire token", ex);
			}
		}
	}
}

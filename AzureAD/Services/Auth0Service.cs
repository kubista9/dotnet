using AzureAD.Configs;
using AzureAD.Models;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Polly;

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
			// Define Polly retry policy
			var retryPolicy = Policy.Handle<Exception>(ex =>
					ex is TimeoutException || ex is HttpRequestException)
				.WaitAndRetryAsync(3, retryAttempt =>
					TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); // Exponential backoff

			return await retryPolicy.ExecuteAsync(async () =>
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

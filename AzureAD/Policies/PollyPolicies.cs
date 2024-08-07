using System.Net;
using AzureAD.Services;
using Polly;

namespace AzureAD.Policies;

public static class PollyPolicies
{

	public static async Task<HttpResponseMessage> ExecuteAsync(Func<CancellationToken, Task<HttpResponseMessage>> execution, CancellationToken cancellationToken)
	{
		return await Policy.Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.Unauthorized)
			.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), async (exception, retryCount, context) =>
			{
				var token = await _tokenService.RefreshTokenAsync();
				// Update request with new token
			})
			.ExecuteAsync(execution, cancellationToken);
	}
}

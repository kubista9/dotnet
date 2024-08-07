using System.Net;
using AzureAD.Services;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace AzureAD.Policies;

public static class PollyPolicies
{
	public static readonly AsyncRetryPolicy RetryPolicy = Policy
		.Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.Unauthorized)
		.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

	 /*
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

	public static readonly AsyncCircuitBreakerPolicy CircuitBreakerPolicy = Policy
		.Handle<HttpRequestException>()
		.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30), OnBreak, OnReset);

	private static async Task OnBreak(Exception ex, TimeSpan wait, Context context)
	{
		// Handle circuit breaker open logic
	}

	private static Task OnReset(Context context)
	{
		// Handle circuit breaker reset logic
		return Task.CompletedTask;
	}
	*/
}

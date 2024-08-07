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
}

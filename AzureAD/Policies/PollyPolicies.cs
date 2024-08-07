using System.Net;
using Polly;
using Polly.Retry;

namespace AzureAD.Policies;

public static class PollyPolicies
{
	public static readonly AsyncRetryPolicy RetryPolicy = Policy
		.Handle<HttpRequestException>(ex => ex.StatusCode == HttpStatusCode.Unauthorized)
		.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

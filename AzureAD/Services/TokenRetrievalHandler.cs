using Polly;
using System.Net.Http.Headers;
using AzureAD.Models;
using AzureAD.Policies;

namespace AzureAD.Services;

public class TokenRetrievalHandler : DelegatingHandler
{
	private readonly ITokenService _tokenService;
	private const string TokenRetrieval = nameof(TokenRetrieval);
	private const string TokenKey = nameof(TokenKey);
	public TokenRetrievalHandler(ITokenService tokenService)
	{
		_tokenService = tokenService;
	}

	protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken, string url)
	{
		if (!request.Properties.TryGetValue(TokenRetrieval, out var context))
		{
			context = new Context(TokenRetrieval, new Dictionary<string, object> { { TokenKey, await _tokenService.GetTokenFromCacheAsync()}});
			request.Properties[TokenRetrieval] = context;
		}

		var token = (Token)((Context)context)[TokenKey];

		if (token != Token.Empty)
			request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.AccessToken);

		//return await base.SendAsync(request, cancellationToken);

		return await PollyPolicies.RetryPolicy.ExecuteAsync(async () =>
		{
			var httpClient = new HttpClient();
			return await httpClient.GetAsync(url, cancellationToken);
		});
	}
}

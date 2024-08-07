using Polly;
using System.Net.Http.Headers;
using AzureAD.Models;

namespace AzureAD.Services;

public class TokenHandler : DelegatingHandler
{
	private readonly ITokenService _tokenService;
	private const string TokenRetrieval = nameof(TokenRetrieval);
	private const string TokenKey = nameof(TokenKey);
	public TokenHandler(ITokenService service)
	{
		_tokenService = service;
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var context = request.GetPolicyExecutionContext();
		if (context.Count == 0)
		{
			context = new Context(TokenRetrieval, new Dictionary<string, object> { { TokenKey, await _tokenService.GetTokenAsync()}});
			request.SetPolicyExecutionContext(context);
		}

		var token = (Token)context[TokenKey];

		if (token != Token.Empty)
			request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.AccessToken);

		return await base.SendAsync(request, cancellationToken);
	}
}

namespace AzureAD.Services;

public class ExternalService<T> : IExternalService<T>
{
	private readonly HttpClient _httpClient;
	private readonly ITokenService _tokenService;

	public ExternalService(HttpClient httpClient, ITokenService tokenService)
	{
		_httpClient = httpClient;
		_tokenService = tokenService;
	}
	public Task<T> GetAsync(string endpoint, Dictionary<string, string> queryParameters)
	{
		throw new NotImplementedException();
	}

	public Task<T> PostAsync<TRequest>(string endpoint, TRequest data, Dictionary<string, string> header)
	{
		throw new NotImplementedException();
	}
}

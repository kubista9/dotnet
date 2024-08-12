using System.Text.Json;
using System.Web;
using AzureAD.Policies;

namespace AzureAD.Services;

public class ExternalService<T> : IExternalService<T>
{
	private readonly HttpClient _httpClient;
	private readonly TokenRetrievalHandler _tokenRetrievalHandler;

	public ExternalService(HttpClient httpClient, TokenRetrievalHandler tokenRetrievalHandler)
	{
		_httpClient = httpClient;
		_tokenRetrievalHandler = tokenRetrievalHandler;
	}
	public async Task<T> GetAsync(string endpoint, Dictionary<string, string>? queryParameters)
	{
		var builder = new UriBuilder(endpoint);

		if (queryParameters != null)
		{
			var query = HttpUtility.ParseQueryString(string.Empty);
			foreach (var parameter in queryParameters)
			{
				query[parameter.Key] = parameter.Value;
			}
			builder.Query = query.ToString();
		}

		var request = new HttpRequestMessage(HttpMethod.Get, builder.Uri);

		using (var response = await _httpClient.SendAsync(request))
		{
			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();

			return JsonSerializer.Deserialize<T>(content);
		}
	}
}

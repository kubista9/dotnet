namespace AzureAD.Services;

public interface IExternalService<T>
{
	Task<T> GetAsync(string endpoint, Dictionary<string, string> queryParameters);
	Task<T> PostAsync<TRequest>(string endpoint, TRequest data, Dictionary<string, string> header);
}

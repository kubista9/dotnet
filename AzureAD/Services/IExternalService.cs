namespace AzureAD.Services;

public interface IExternalService<T>
{
	Task<T> GetAsync(string endpoint, Dictionary<string, string> queryParameters);
}

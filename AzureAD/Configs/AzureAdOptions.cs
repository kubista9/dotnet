namespace AzureAD.Configs;

public class AzureAdOptions
{
	public string ClientId { get; set; }
	public string ClientSecret { get; set; }
	public string TenantId { get; set; }
	public string Scope { get; set; }
	public string Authority => $"https://login.microsoftonline.com/{TenantId}";
}


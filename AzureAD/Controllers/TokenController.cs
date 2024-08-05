using AzureAD.Models;
using AzureAD.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureAD.Controllers;

public class TokenController : ControllerBase
{
	private readonly IAuth0Service _auth0Service;

	public TokenController(IAuth0Service auth0Service)
	{
		_auth0Service = auth0Service;
	}

	[HttpGet("token")]
	public async Task<IActionResult> GetToken()
	{
		var token = await _auth0Service.GetTokenAsync();
		return Ok(token);
	}
}

# Azure AD is a C# .NET Core Web Api project used for retrieving, storing and forwarding an Access Token

## Project has Azure AD integration, caching & uses Polly for resilience

This project was used for learning about JWTs, APIs, Polly, C# .NET, Cache and Swagger. It allows you to:
- Get a JWT token from Azure AD
- Add an external service
- Store the JWT token in a cache
- Retrieve the JWT token from a cache
- Access the endpoints with Swagger UI

## Visual Helper

### Class Diagram
![Class diagram](ClassDiagram.drawio.svg)

### Explanation
- TokenController contains the API endpoints
- Auth0CService has the logic for interacting with Azure AD to GET an Access Token using Polly
- ExternalService is a generic class that provides a GET method for making HTTP requests to external services
- TokenRetrievalHandler is a transient class. This HTTP message handler adds an authorization header to outgoing requests
- TokenService manages the in-memory cache for the Access Token
- PollyPolicies is a static class that contains a retry policy for HTTP requests that result in a 401 (Unauthorized) status code

## Instructions

### Prerequisites

- .NET Core SDK
- Azure Active Directory application

### Developer Instructions

#### 1. Configure the Azure AD application settings in appsettings.json:
	"AzureAd": {
		"ClientSecret": "your-client-secret",
		"ClientId": "your-client-id",
		"TenantId": "your-tenant-id",
		"Scope": "your-scope"
	}

#### 2. Install the required NuGet packages:
	dotnet add package Microsoft.AspNetCore.Controllers.Abstractions
	dotnet add package Microsoft.Extensions.Caching.Memory
	dotnet add package Microsoft.Identity.Client
	dotnet add package Microsoft.AspNetCore.Mvc
	dotnet add package Polly

#### 3. Build and run the API:
	dotnet run

#### 4. Access the endpoints

With Swagger UI
http://localhost:5225/swagger/index.html

GET a token from Azure AD
http://localhost:5225/tokenFromAzure

GET a token from the cache
http://localhost:5225/tokenFromCache

GET data from an external URL
http://localhost:5225/data/{url}




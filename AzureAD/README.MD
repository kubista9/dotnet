# Azure AD is a C# .NET Core Web Api project used for retrieving, storing and forwarding an Access token

## Project has Azure AD integration, caching & uses Polly for resilience

This project was used for learning about JWT, APIs, Polly, C# .NET, Cache and Swagger. It allows you to:
- Get a JWT token from Azure AD
- Store a JWT token in a cache
- Retrieve the JWT token from a cache
- Add an external service to interact with the system
- Access the Swagger UI

## Diagrams

### Class Diagram
![Class diagram](ClassDiagram.drawio.svg)

- TokenController contains the API endpoints
- Auth0CService has the logic for interacting with Azure AD to retrieve tokens using Polly
- TokenService manages the in-memory cache for access tokens
- ExternalService is a generic class that provides a GET method for making HTTP requests to external services
- TokenRetrievalHandler is a transient class. This HTTP message handler adds an authorization header to outgoing requests if a token is available in the cache
- PollyPolicies is a static class that contains a retry policy for HTTP requests that result in a 401 (Unauthorized) status code





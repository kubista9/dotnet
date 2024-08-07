using AzureAD.Configs;
using AzureAD.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AzureAdOptions>(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddScoped<IAuth0Service, Auth0Service>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // Map the controllers

app.Run();

using OpenTelemetry.Trace;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container from the Aspire ServiceDefaults
builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

// add services to the container from the Aspire AppHost
builder.AddMongoDBClient("mongodb");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add telemetry services
// These can come from a config file, constants file, etc.
var serviceName = "FlowBiteAdmin.API";

// Add TracerProvider. This will get injected into the WeatherForecastController
builder.Services.AddSingleton(TracerProvider.Default.GetTracer(serviceName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// We need this for local development as the frontend page is running on a different port
// This is the page the required this change: \AstroFrontend\src\pages\weatherapidirect.astro
// Production SHOULD control the CORS policy within the API Container App itself.
app.UseCors( x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseAuthorization();

app.MapControllers();

// This will add the default health checks from the ServiceDefaults
// MS Docs: https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/health-checks
// Implementation: AstroAspire.ServiceDefaults\Extensions.cs
// Will expose these paths at the base of the AstroAspire.API:
//      /health
//      /alive
app.MapDefaultEndpoints();


app.Run();

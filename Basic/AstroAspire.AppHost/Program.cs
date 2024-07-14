var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.AstroAspire_API>("api")
    .WithExternalHttpEndpoints();

// As a reference, we could use the AddNodeApp method to add the frontend project
// However, we'll use the AddNpmApp method to add the frontend project
// Below is an example if we were to use the AddNodeApp method
// var frontend = builder.AddNodeApp("frontend", "../AstroFrontend/app.js")

// Add the frontend project to the builder
// Let's explain the parameters of the AddNpmApp method
// 1. The name of the resource that Aspire will then reference in the Dashboard: "frontend"
// 2. The path to the frontend project: "../AstroFrontend"
// 3. The name of the npm script to run (defined in ../AstroFrontend/package.json): "aspirerun"

var frontend = builder.AddNpmApp("frontend", "../AstroFrontend", "aaWatch")
    .WithReference(api)
    // Parameter is defined in the appsettings.json file
    // Since the API path could change in the API project, we'll pass it as a parameter to the frontend project
    .WithEnvironment("apiBasePath", "/api")
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    //.WithEnvironment("OTEL_NODE_RESOURCE_DETECTORS", "env, host, os, process, serviceinstance, container, azure")
    // Disable the fs instrumentation as it's not needed for the frontend project and sends too much to tracing
    .WithEnvironment("OTEL_NODE_DISABLED_INSTRUMENTATIONS", "fs")
    .PublishAsDockerFile();

// Let's pass the frontend reference so we can enable CORS to accept calls to the API.
//  The frontend project will be running on a different port than the API project.
//  We only need this for the /weatherapidirect page which used client side Javascript to call the AstroAspire.API
api.WithReference(frontend);

Console.WriteLine("builder.Environment.EnvironmentName: " + builder.Environment.EnvironmentName);

// This is needed in development as we don't have a valid certificate for the frontend to call the AstroAspire.API without failing
if (builder.Environment.EnvironmentName == "Development" && builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    // Disable TLS certificate validation in development, see https://github.com/dotnet/aspire/issues/3324 for more details.
    frontend.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

builder.Build().Run();

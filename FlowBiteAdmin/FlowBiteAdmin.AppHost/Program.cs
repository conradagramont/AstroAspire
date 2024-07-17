var builder = DistributedApplication.CreateBuilder(args);

// We're going to use an Aspire to add the MongoDB client
var mongo = builder.AddMongoDB("mongo")
    .WithMongoExpress()
    .AddDatabase("mongodb", "FlowBiteAdmin");

// Add the API project to the builder
 var api = builder.AddProject<Projects.FlowBiteAdmin_API>("api")
    .WithReference(mongo)
    .WithExternalHttpEndpoints();
    //.WithHttpEndpoint(env: "PORT");

// Add the frontend project to the builder
// Let's explain the parameters of the AddNpmApp method
// 1. The name of the resource that Aspire will then reference in the Dashboard: "frontend"
// 2. The path to the frontend project: "../AstroFrontend"
// 3. The name of the npm script to run (defined in ../AstroFrontend/package.json): "aspirerun"

var frontend = builder.AddNpmApp("frontend", "../Frontend", "aaStart")
    .WithReference(api)
    // Parameter is defined in the appsettings.json file
    // Since the API path could change in the API project, we'll pass it as a parameter to the frontend project
    .WithEnvironment("BROWSER", "none") // Disable opening browser on npm start
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    //.WithEnvironment("OTEL_NODE_RESOURCE_DETECTORS", "env, host, os, process, serviceinstance, container, azure")
    // Disable the fs instrumentation as it's not needed for the frontend project and sends too much to tracing
    .WithEnvironment("OTEL_NODE_DISABLED_INSTRUMENTATIONS", "fs")
    .PublishAsDockerFile();

var adminUI = builder.AddProject<Projects.FlowBiteAdmin_RazorUI>("adminUI")
    .WithReference(mongo)
    .WithExternalHttpEndpoints();

var dbManager = builder.AddProject<Projects.FlowBiteAdmin_DBManager>("dbManager")
    .WithReference(mongo);

// This is needed in development as we don't have a valid certificate for the frontend to call the AstroAspire.API without failing
if (builder.Environment.EnvironmentName == "Development" && builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    // Disable TLS certificate validation in development, see https://github.com/dotnet/aspire/issues/3324 for more details.
    frontend.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

builder.Build().Run();

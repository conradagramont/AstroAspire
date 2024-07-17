using FlowBiteAdmin.DBManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container from the Aspire ServiceDefaults
builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

// add services to the container from the Aspire AppHost
builder.AddMongoDBClient("mongodb");

// Add telemetry services (OpenTelemetry) in aligment with ServiceDefaults
builder.Services.AddOpenTelemetry()
   .WithTracing(tracing => tracing.AddSource(DBInitializer.ActivitySourceName));

builder.Services.AddSingleton<DBInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<DBInitializer>());
builder.Services.AddHealthChecks()
    .AddCheck<DBInitHealthCheck>("DBInitializer", null);

// Add services to the container.
//builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapDefaultEndpoints();

/*
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
*/

app.Run();

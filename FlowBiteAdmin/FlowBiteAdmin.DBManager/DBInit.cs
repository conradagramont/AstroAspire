using System.Diagnostics;
using MongoDB.Driver;
using FlowBiteAdmin.Shared;

namespace FlowBiteAdmin.DBManager;

internal class DBInitializer(IMongoClient mongoClient, ILogger<DBInitializer> logger) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private readonly IMongoClient _mongoClient = mongoClient;

    private readonly ActivitySource _activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {

        await InitializeDatabaseAsync(cancellationToken);
    }

    private async Task InitializeDatabaseAsync(CancellationToken cancellationToken)
    {
        using var activity = _activitySource.StartActivity("Initializing catalog database", ActivityKind.Client);

        var sw = Stopwatch.StartNew();

        await SeedAsync(cancellationToken);

        logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
    }

    private async Task SeedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Seeding database");

        var users = Enumerable.Range(1, 5).Select(index => new User
        {
            id = index,
            name = "John Doe",
            email = "j.d@fake.com",
            avatar = "https://via.placeholder.com/150",
            biography = "This is a biography",
            position = "Developer",
            country = "USA",
            status = "Active"
        })
        .ToArray();


        var sw = Stopwatch.StartNew();

        var database = _mongoClient.GetDatabase("FlowBiteAdmin");
        var collection = database.GetCollection<User>("Users");

        await collection.InsertManyAsync(users);
    }
}
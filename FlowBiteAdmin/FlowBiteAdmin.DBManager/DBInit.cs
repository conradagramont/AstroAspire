using System.Diagnostics;
using MongoDB.Driver;
using FlowBiteAdmin.Shared;

namespace FlowBiteAdmin.DBManager;

internal class DBInitializer(IMongoClient mongoClient, ILogger<DBInitializer> logger) : BackgroundService
{
    public const string ActivitySourceName = "Seeding";
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
        var database = _mongoClient.GetDatabase("FlowBiteAdmin");
        var collection = database.GetCollection<User>("Users");

        if (await collection.CountDocumentsAsync(FilterDefinition<User>.Empty) > 0)
        {
            logger.LogInformation("Database already seeded");
            return;
        }
        else
        {
            logger.LogInformation("Seeding database");

            int batchSize = 1000;
            int batchCount = 50000 / batchSize;
            int remaining = 50000 % batchSize;

            // Loop through the batches and insert them into the database
            for (int i = 1; i < batchCount; i++)
            {
                logger.LogInformation("Inserting batch {BatchIndex} of {BatchCount}", i + 1, batchCount);
                var users = Enumerable.Range(i * batchSize, batchSize).Select(index => new User
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

                await collection.InsertManyAsync(users);
            }
            var sw = Stopwatch.StartNew();
        }
    }
}
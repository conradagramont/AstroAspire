using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using FlowBiteAdmin.Shared;

namespace FlowBiteAdmin.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{   
    private readonly ILogger<UserController> _logger;
    private readonly IMongoClient _mongoClient;

    public UserController(ILogger<UserController> logger, IMongoClient mongoClient)
    {
        _logger = logger;
        _mongoClient = mongoClient;
    }

    [HttpGet]
    public IEnumerable<User> Get()
    {
        var database = _mongoClient.GetDatabase("FlowBiteAdmin");
        var collection = database.GetCollection<User>("Users");
        
        var users = collection.Find(FilterDefinition<User>.Empty).Limit(10).ToList();

        return users;

    }
    [HttpGet("{id}")]
    public User GetUser(string id)
    {
        //convert id to int
        int idInt = Int32.Parse(id);

        var database = _mongoClient.GetDatabase("FlowBiteAdmin");
        var collection = database.GetCollection<User>("Users");

        // Search for user based on the passed in userID and return if found
        var builder = Builders<User>.Filter;
        var idFilter = builder.Eq(u => u.id, idInt);
        var cursor = collection.Find(idFilter);
        var user = cursor.FirstOrDefault();
        if (user == null) {
            return null;
        }
        return user;

    }

    [HttpPost(Name = "AddUser")]
    public User AddUser(User user)
    {
        // Add the user to the database


        return user;
    }

}
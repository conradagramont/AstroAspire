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

    [HttpGet(Name = "GetUsers")]
    public IEnumerable<User> Get()
    {
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

        var database = _mongoClient.GetDatabase("FlowBiteAdmin");
        var collection = database.GetCollection<User>("Users");
        collection.InsertManyAsync(users);
        return users;

    }
    [HttpPost(Name = "AddUser")]
    public User AddUser(User user)
    {
        // Add the user to the database


        return user;
    }

}
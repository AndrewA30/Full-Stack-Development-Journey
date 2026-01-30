using Microsoft.AspNetCore.OpenApi;
using Swashbuckle.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IMyService, MyService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


var Blogs = new List<Blog>{
    new Blog{ Title="First Post", Body="This is my first blog post."},
    new Blog{ Title="Second Post", Body="This is my second blog post."}
};

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use(async (context, next) =>
{
    var start = DateTime.UtcNow;
    await next();
    var duration = DateTime.UtcNow - start;
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path} took {duration.TotalMilliseconds} ms");
});

app.UseWhen(
    context => context.Request.Method != "GET",
    appBuilder => appBuilder.Use(async (context, next) =>
    {
        var extractedPassword = context.Request.Headers["X-Password"];
        if (extractedPassword == "SecretPassword")
        {
            await next();
            return;
        }else
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized: Invalid or missing password.");
            return;
        }
    })
);

app.MapGet("/", () => "Hello World!");

app.MapGet("/blogs", () => Blogs);

app.MapGet("/blogs/{id}", (int id) => 
{
    if (id < 0 || id >= Blogs.Count)
        return Results.NotFound("Blog not found.");
    return Results.Ok(Blogs[id]);
}).AddOpenApiOperationTransformer((operation,context,ct) => 
{
    operation.Summary = "Get a blog by its ID";
    operation.Description = "Returns a single blog post based on the provided ID.";
    return Task.FromResult(operation);
});

app.MapPost("/blogs", (Blog blog) => 
{
    Blogs.Add(blog);
    return Results.Created($"/blogs/{Blogs.Count - 1}", blog);
}); 

app.MapPut("/blogs/{id}", (int id, Blog updatedBlog) => 
{
    if (id < 0 || id >= Blogs.Count)
        return Results.NotFound("Blog not found.");
    Blogs[id] = updatedBlog;
    return Results.Ok(updatedBlog);
});

app.MapDelete("/blogs/{id}", (int id) => 
{
    if (id < 0 || id >= Blogs.Count)
        return Results.NotFound("Blog not found.");
    Blogs.RemoveAt(id);
    return Results.Ok("Blog deleted.");
});

app.MapGet("/service-info", (IMyService myService) => 
{
    myService.LogCreation("Service instance accessed.");
    return Results.Ok("Service instance logged.");
});

app.Run();

public class Blog 
{
    public required string Title { get; set; }
    public required string Body { get; set; }
}

public interface IMyService
{
    void LogCreation(string message);
}

public class MyService : IMyService
{
    private readonly int _serviceId;

    public MyService()
    {
        _serviceId = new Random().Next(100000, 999999);
    }

    public void LogCreation(string message)
    {
        Console.WriteLine($"Service ID: {_serviceId} - {message}");
    }
}
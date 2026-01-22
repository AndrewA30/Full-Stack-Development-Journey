var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// Example of static response endpoints
app.MapGet("/", () => "Hello Woi!");
app.MapGet("/downloads", () => "Downloads Page");
app.MapPut("/", () => "This is put mentoh");
app.MapPost("/", () => "This is post mentoh");
app.MapDelete("/", () => "This is delete mentoh");

// Example of dynamic response endpoints
app.MapGet("/user/{name}/posts/{age}", (string name, int age) => $"Hello, {name}! You are {age}.");
// example of dynamic response endpoints with constraints
app.MapGet("/product/{name}/posts/{number:int:min(1)}", (string name, int number) => $"Hello, {name}! You are {number}.");
// example of dynamic response endpoints with optional parameters
app.MapGet("/book/{name}/posts/{id?}", (string name, int? id) =>
{
    if (id.HasValue)
    {
        return $"Hello, {name}! You are {id}.";
    }
    else
    {
        return $"Hello, {name}! ID not provided.";
    }
});
// example of dynamic response endpoints to get file path
app.MapGet("/files/{*filepath}", (string filepath) => $"File path requested: {filepath}");          
// example of dynamic response endpoints with query parameters to test it in postman the url is like this: https://localhost:7242/search?query=example&page=2
app.MapGet("/search", (string? query, int? page) =>
{
    if (page.HasValue)
    {
        return $"Search results for '{query}' on page {page}.";
    }
    else
    {
        return $"Search results for '{query}' on page 1.";
    }
});
app.Run();

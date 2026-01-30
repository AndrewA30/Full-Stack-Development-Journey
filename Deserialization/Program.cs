using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/person", (Person person) =>
{
    return Results.Ok(person);
});

app.MapPost("/json", async (HttpRequest request) =>
{
    var person = await request.ReadFromJsonAsync<Person>();
    return Results.Ok(person);
});

app.MapPost("/custom-options", async (HttpRequest request) =>
{
    // var options = new System.Text.Json.JsonSerializerOptions
    // {
    //     UnmappedMemberHandling = JsonUnmappedMemberHandling.Ignore
    // };

    var person = await request.ReadFromJsonAsync<Person>();
    return Results.Ok(person);
});

app.MapPost("/xml", async (HttpRequest request) =>
{
    var reader = new System.IO.StreamReader(request.Body);
    var Body   = await reader.ReadToEndAsync();

    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Person));
    var person = (Person?)serializer.Deserialize(request.Body);
    return Results.Ok(person);
});

app.Run();

public class Person
{
    required public string UserName { get; set; }
    public int? UserAge { get; set; }
}
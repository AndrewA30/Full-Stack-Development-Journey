var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var samplePerson = new Person
{
    UserName = "Alice",
    UserAge = 30
};

app.MapGet("/", () => "Hello World!");
app.MapGet("/manual-json", () => {
    var json = System.Text.Json.JsonSerializer.Serialize(samplePerson);
    return Results.Content(json, "application/json");
});

app.MapGet("/automatic-json", () => samplePerson);

app.MapGet("/custom-json", () => {
    var options = new System.Text.Json.JsonSerializerOptions
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower
    };
    var json = System.Text.Json.JsonSerializer.Serialize(samplePerson, options);
    return Results.Content(json, "application/json");
});

app.MapGet("/xml", () => {
    var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Person));
    using var stringWriter = new System.IO.StringWriter();
    xmlSerializer.Serialize(stringWriter, samplePerson);
    var xml = stringWriter.ToString();
    return Results.Content(xml, "application/xml");
});

app.Run();

public class Person
{
    required public string UserName { get; set; }
    required public int UserAge { get; set; }
}
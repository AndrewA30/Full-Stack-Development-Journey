var builder = WebApplication.CreateBuilder(args);
// this is to enable logging for HttpLoggingMiddleware dont forget to add settings in appsettings.json => "Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware": "Information"
builder.Services.AddHttpLogging((o) => {});
var app = builder.Build();

app.UseHttpLogging();

app.Use(async (context, next) =>
{
    // Custom Middleware Logic Before
    Console.WriteLine("Logic Before Next Middleware");
    await next.Invoke();
    // Custom Middleware Logic After
    Console.WriteLine("Logic After Next Middleware");
});

app.MapGet("/", () => "Hello World!");

app.MapGet("/test", () => "This is a test endpoint.");

app.Run();

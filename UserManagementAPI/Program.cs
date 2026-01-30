using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseExceptionHandling();
app.UseTokenValidation(); 
app.UseRequestLogging();


// In-memory user store
var users = new List<User>
{
    new User { Id = 1, FirstName = "Alice", LastName = "Johnson", Email = "alice@techhive.com", Department = "HR" },
    new User { Id = 2, FirstName = "Bob", LastName = "Smith", Email = "bob@techhive.com", Department = "IT" }
};

// GET: Retrieve all users
app.MapGet("/users", () =>
{
    try
    {
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected error: {ex.Message}");
    }
});

// GET: Retrieve a user by ID
app.MapGet("/users/{id}", (int id) =>
{
    if (id <= 0) return Results.BadRequest("Invalid user ID.");
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// POST: Add a new user
app.MapPost("/users", (User newUser) =>
{
    if (string.IsNullOrWhiteSpace(newUser.FirstName) ||
        string.IsNullOrWhiteSpace(newUser.LastName) ||
        string.IsNullOrWhiteSpace(newUser.Email) ||
        string.IsNullOrWhiteSpace(newUser.Department))
    {
        return Results.BadRequest("All fields are required.");
    }

    if (users.Any(u => u.Email == newUser.Email))
    {
        return Results.BadRequest("Email already exists.");
    }

    newUser.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
    users.Add(newUser);
    return Results.Created($"/users/{newUser.Id}", newUser);
});

// PUT: Update an existing user
app.MapPut("/users/{id}", (int id, User updatedUser) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();

    user.FirstName = updatedUser.FirstName;
    user.LastName = updatedUser.LastName;
    user.Email = updatedUser.Email;
    user.Department = updatedUser.Department;

    return Results.NoContent();
});

// DELETE: Remove a user by ID
app.MapDelete("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user is null) return Results.NotFound();

    users.Remove(user);
    return Results.NoContent();
});

app.Run();

// User model
record User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Department { get; set; } = default!;
}

// --------------------
// Middleware definition
// --------------------
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method;
        var path = context.Request.Path;

        _logger.LogInformation("Incoming Request: {Method} {Path}", method, path);

        await _next(context);

        var statusCode = context.Response.StatusCode;
        _logger.LogInformation("Outgoing Response: {StatusCode}", statusCode);
    }
}

// --------------------
// Extension method
// --------------------
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Pass request down the pipeline
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "Unhandled exception occurred.");

            // Return consistent JSON error response
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new { error = "Internal server error." };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenValidationMiddleware> _logger;

    // For demo purposes, we’ll use a hardcoded token.
    // In production, integrate with JWT or OAuth.
    private const string ValidToken = "my-secret-token";

    public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check for Authorization header
        if (!context.Request.Headers.TryGetValue("Authorization", out var token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Unauthorized: Missing token." });
            return;
        }

        // Validate token (simple equality check here)
        if (token != $"Bearer {ValidToken}")
        {
            _logger.LogWarning("Invalid token provided.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Unauthorized: Invalid token." });
            return;
        }

        // Token is valid → continue pipeline
        await _next(context);
    }
}
public static class TokenValidationMiddlewareExtensions
{
    public static IApplicationBuilder UseTokenValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenValidationMiddleware>();
    }
}
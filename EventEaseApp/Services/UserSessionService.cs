using EventEaseApp.Models;
using System.Text.Json;
using Microsoft.JSInterop;

namespace EventEaseApp.Services;

public class UserSessionService
{
    private readonly IJSRuntime _jsRuntime;
    private const string SESSION_KEY = "eventease_user_session";
    private const string REGISTRATIONS_KEY = "eventease_registrations";

    public event Action? OnSessionChanged;

    public UserSessionService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    // Get current logged-in user
    public async Task<UserSession?> GetCurrentUserAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", SESSION_KEY);
            if (string.IsNullOrEmpty(json))
                return null;

            return JsonSerializer.Deserialize<UserSession>(json);
        }
        catch
        {
            return null;
        }
    }

    // Save user session
    public async Task SaveUserSessionAsync(UserSession user)
    {
        try
        {
            var json = JsonSerializer.Serialize(user);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", SESSION_KEY, json);
            OnSessionChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving user session: {ex.Message}");
        }
    }

    // Clear user session
    public async Task ClearUserSessionAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", SESSION_KEY);
            OnSessionChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing user session: {ex.Message}");
        }
    }

    // Add registration to user's registration list
    public async Task AddRegistrationAsync(EventRegistration registration)
    {
        try
        {
            var registrations = await GetUserRegistrationsAsync();
            registrations.Add(registration);

            var json = JsonSerializer.Serialize(registrations);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", REGISTRATIONS_KEY, json);
            OnSessionChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding registration: {ex.Message}");
        }
    }

    // Get all user registrations
    public async Task<List<EventRegistration>> GetUserRegistrationsAsync()
    {
        try
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", REGISTRATIONS_KEY);
            if (string.IsNullOrEmpty(json))
                return new List<EventRegistration>();

            return JsonSerializer.Deserialize<List<EventRegistration>>(json) ?? new List<EventRegistration>();
        }
        catch
        {
            return new List<EventRegistration>();
        }
    }

    // Get registrations for specific event
    public async Task<List<EventRegistration>> GetEventRegistrationsAsync(int eventId)
    {
        var allRegistrations = await GetUserRegistrationsAsync();
        return allRegistrations.Where(r => r.EventId == eventId).ToList();
    }

    // Check if user is registered for event
    public async Task<bool> IsUserRegisteredForEventAsync(int eventId)
    {
        var registrations = await GetEventRegistrationsAsync(eventId);
        return registrations.Count > 0;
    }

    // Get user registration count
    public async Task<int> GetUserRegistrationCountAsync()
    {
        var registrations = await GetUserRegistrationsAsync();
        return registrations.Count;
    }

    // Update user profile
    public async Task UpdateUserProfileAsync(string firstName, string lastName, string email, string phone)
    {
        var user = await GetCurrentUserAsync() ?? new UserSession();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.PhoneNumber = phone;
        user.LastUpdated = DateTime.Now;

        await SaveUserSessionAsync(user);
    }
}


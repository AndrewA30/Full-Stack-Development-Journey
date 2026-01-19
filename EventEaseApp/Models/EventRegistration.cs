namespace EventEaseApp.Models;

public class EventRegistration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
    public string RegistrationStatus { get; set; } = "Confirmed";
    public string? SpecialRequirements { get; set; }
}

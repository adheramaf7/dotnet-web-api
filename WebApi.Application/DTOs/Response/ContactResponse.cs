namespace WebApi.Application.DTOs.Response
{
    public class ContactResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
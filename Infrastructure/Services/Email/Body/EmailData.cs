namespace Infrastructure.Services.Email.Body
{
    public class EmailData
    {
        public required string To { get; set; }
        public required string Subject { get; set; }
        public string? Body { get; set; }
    }
}

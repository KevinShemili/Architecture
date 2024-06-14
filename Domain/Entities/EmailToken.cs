using Domain.Common;

namespace Domain.Entities
{
    public class EmailToken : AuditableEntityBase
    {
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}

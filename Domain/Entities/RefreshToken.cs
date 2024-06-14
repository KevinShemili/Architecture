using Domain.Common;

namespace Domain.Entities
{
    public class RefreshToken : AuditableEntityBase
    {
        public string Token { get; set; }
        public DateTime Expiry {  get; set; }
        public Guid JwtTokenId { get; set; }        
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}

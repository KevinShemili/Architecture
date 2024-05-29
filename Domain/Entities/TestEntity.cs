using Domain.Common;

namespace Domain.Entities
{
    public class TestEntity : AuditableEntityBase
    {
        public string? TestString { get; set; }
        public decimal TestDecimal { get; set; }
    }
}

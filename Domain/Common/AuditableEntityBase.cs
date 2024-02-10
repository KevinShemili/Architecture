namespace Domain.Common {
	public abstract class AuditableEntityBase : EntityBase {
		public DateTime DateCreated { get; set; }
		public DateTime? DateUpdated { get; set; }
	}
}

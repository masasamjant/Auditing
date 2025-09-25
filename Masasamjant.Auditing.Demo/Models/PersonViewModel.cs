namespace Masasamjant.Auditing.Demo.Models
{
    public class PersonViewModel
    {
        [AuditedProperty(true, true)]
        public Guid Identifier { get; set; } = Guid.NewGuid();

        [AuditedProperty(true)]
        public string FirstName { get; set; } = string.Empty;

        [AuditedProperty(true)]
        public string LastName { get; set; } = string.Empty;
    }
}

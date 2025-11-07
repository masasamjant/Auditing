using Masasamjant.Auditing.Abstractions;

namespace Masasamjant.Auditing
{
    public class Person
    {
        public Person()
        {
            Identifier = Guid.NewGuid();
        }

        [AuditedProperty(true, true)]
        public Guid Identifier { get; }

        [AuditedProperty(true, AuditingActionType.View, AuditingActionType.Add, AuditingActionType.Update)]
        public string FirstName { get; set; } = string.Empty;

        [AuditedProperty(true, AuditingActionType.View, AuditingActionType.Add, AuditingActionType.Update)]
        public string LastName { get; set; } = string.Empty;
    }

    public class Address : IProvideAuditingKeys
    {
        public Address()
        {
            Identifier = Guid.NewGuid();
        }
        public Guid Identifier { get; }

        [AuditedProperty(true, AuditingActionType.View, AuditingActionType.Add, AuditingActionType.Update)]
        public string StreetAddress { get; set; } = string.Empty;

        [AuditedProperty(true, AuditingActionType.View, AuditingActionType.Add, AuditingActionType.Update)]
        public string Country { get; set; } = string.Empty;

        object[] IProvideAuditingKeys.GetAuditingKeys()
        {
            return [Identifier];
        }
    }
}

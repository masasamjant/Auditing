using System.Text.Json.Serialization;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents an auditing event.
    /// </summary>
    public class AuditingEvent
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="AuditingEvent"/> class with single autited object.
        /// </summary>
        /// <param name="identifier">The unique identifier of the event.</param>
        /// <param name="eventAction">The <see cref="AuditingAction"/>.</param>
        /// <param name="eventUser">The <see cref="AuditingUser"/>.</param>
        /// <param name="auditedObject">The <see cref="AuditedObjectDescriptor"/>.</param>
        /// <exception cref="ArgumentException">If <paramref name="identifier"/> is <see cref="Guid.Empty"/>.</exception>
        public AuditingEvent(Guid identifier, AuditingAction eventAction, AuditingUser eventUser, AuditedObjectDescriptor auditedObject)
            : this(identifier, eventAction, eventUser, [auditedObject])
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditingEvent"/> class with multiple audited objects. Note that the action is expected to be the same for all objects.
        /// Like when user views list of products, the action is "View" and all products are listed as audited objects.
        /// </summary>
        /// <param name="identifier">The unique identifier of the event.</param>
        /// <param name="eventAction">The <see cref="AuditingAction"/>.</param>
        /// <param name="eventUser">The <see cref="AuditingUser"/>.</param>
        /// <param name="auditedObjects">The audited object involved to action.</param>
        /// <exception cref="ArgumentException">If <paramref name="identifier"/> is <see cref="Guid.Empty"/>.</exception>
        public AuditingEvent(Guid identifier, AuditingAction eventAction, AuditingUser eventUser, IEnumerable<AuditedObjectDescriptor> auditedObjects)
        {
            if (Guid.Empty.Equals(identifier))
                throw new ArgumentException($"The identifier cannot be empty i.e. '{Guid.Empty}'.", nameof(identifier));

            Identifier = identifier;
            Action = eventAction;
            User = eventUser;
            if (auditedObjects.Any())
                AuditedObjects.AddRange(auditedObjects);
        }

        /// <summary>
        /// Initializes a new empty instance of the <see cref="AuditingEvent"/> class.
        /// </summary>
        public AuditingEvent()
        { }

        /// <summary>
        /// Gets the unique identifier of the event.
        /// </summary>
        [JsonInclude]
        public Guid Identifier { get; internal set; }

        /// <summary>
        /// Gets the <see cref="AuditingAction"/> that represents the action performed.
        /// </summary>
        [JsonInclude]
        public AuditingAction Action { get; internal set; } = new AuditingAction();

        /// <summary>
        /// Gets the <see cref="AuditingUser"/> that represents the user who performed the action.
        /// </summary>
        [JsonInclude]
        public AuditingUser User { get; internal set; } = new AuditingUser();

        /// <summary>
        /// Gets the list of <see cref="AuditedObjectDescriptor"/> that represent the objects involved in the action.
        /// </summary>
        [JsonInclude]
        public List<AuditedObjectDescriptor> AuditedObjects { get; internal set; } = new List<AuditedObjectDescriptor>();
    }
}

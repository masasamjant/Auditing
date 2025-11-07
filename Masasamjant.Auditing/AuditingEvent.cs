using System.Collections;
using System.Text.Json.Serialization;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents an auditing event.
    /// </summary>
    public class AuditingEvent
    {
        private List<AuditedObjectDescriptor> objects = new List<AuditedObjectDescriptor>();

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
                objects.AddRange(auditedObjects);
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
        public IReadOnlyCollection<AuditedObjectDescriptor> AuditedObjects
        {
            get { return objects.AsReadOnly(); }
            internal set { objects = value.ToList(); }
        }

        /// <summary>
        /// Creates <see cref="AuditingEvent"/> for specified audited object instance.
        /// </summary>
        /// <param name="instance">The audited object instance.</param>
        /// <param name="action">The <see cref="AuditingAction"/>.</param>
        /// <param name="user">The <see cref="AuditingUser"/>.</param>
        /// <returns>A <see cref="AuditingEvent"/>.</returns>
        public static AuditingEvent CreateForOne(object instance, AuditingAction action, AuditingUser user)
        {
            return new AuditingEvent(Guid.NewGuid(), action, user, AuditedObjectDescriptor.Create(instance, action.ActionType));
        }

        /// <summary>
        /// Creates <see cref="AuditingEvent"/> for specified audited object instance.
        /// </summary>
        /// <param name="instance">The audited object instance.</param>
        /// <param name="applicationName">The application name.</param>
        /// <param name="actionType">The <see cref="AuditingActionType"/>.</param>
        /// <param name="actionResult">The <see cref="AuditingActionResult"/>.</param>
        /// <param name="user">The <see cref="AuditingUser"/>.</param>
        /// <param name="actionTime">The action time or <c>null</c> to use <see cref="DateTimeOffset.UtcNow"/>.</param>
        /// <param name="faultedMessage">The message that descibes why action faulted.</param>
        /// <returns>A <see cref="AuditingEvent"/>.</returns>
        public static AuditingEvent CreateForOne(object instance, string applicationName, AuditingActionType actionType, AuditingActionResult actionResult, AuditingUser user, DateTimeOffset? actionTime = null, string? faultedMessage = null)
            => CreateForOne(instance, new AuditingAction(applicationName, actionType, actionResult, actionTime, faultedMessage), user);

        /// <summary>
        /// Creates <see cref="AuditingEvent"/> for specified audited object instances.
        /// </summary>
        /// <param name="instances">The audited object instances.</param>
        /// <param name="action">The <see cref="AuditingAction"/>.</param>
        /// <param name="user">The <see cref="AuditingUser"/>.</param>
        /// <returns>A <see cref="AuditingEvent"/>.</returns>
        public static AuditingEvent CreateForMany(IEnumerable instances, AuditingAction action, AuditingUser user)
        { 
            var descriptors = new List<AuditedObjectDescriptor>();

            foreach (var instance in instances)
                descriptors.Add(AuditedObjectDescriptor.Create(instance, action.ActionType));

            return new AuditingEvent(Guid.NewGuid(), action, user, descriptors);
        }

        /// <summary>
        /// Creates <see cref="AuditingEvent"/> for specified audited object instances.
        /// </summary>
        /// <param name="instances">The audited object instances.</param>
        /// <param name="applicationName">The application name.</param>
        /// <param name="actionType">The <see cref="AuditingActionType"/>.</param>
        /// <param name="actionResult">The <see cref="AuditingActionResult"/>.</param>
        /// <param name="user">The <see cref="AuditingUser"/>.</param>
        /// <param name="actionTime">The action time or <c>null</c> to use <see cref="DateTimeOffset.UtcNow"/>.</param>
        /// <param name="faultedMessage">The message that descibes why action faulted.</param>
        /// <returns>A <see cref="AuditingEvent"/>.</returns>
        public static AuditingEvent CreateForMany(IEnumerable instances, string applicationName, AuditingActionType actionType, AuditingActionResult actionResult, AuditingUser user, DateTimeOffset? actionTime = null, string? faultedMessage = null)
            => CreateForMany(instances, new AuditingAction(applicationName, actionType, actionResult, actionTime, faultedMessage), user);
    }
}

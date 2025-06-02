namespace Masasamjant.Auditing
{
    /// <summary>
    /// Attribute applied to properties of audited object instance to indicate that property should be audited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class AuditedPropertyAttribute : Attribute
    {
        /// <summary>
        /// Initializes new instance of the <see cref="AuditedPropertyAttribute"/> class with <see cref="IsKeyProperty"/> as <c>false</c>.
        /// </summary>
        /// <param name="audited"><c>true</c> if property is audited; <c>false</c> otherwise.</param>
        /// <param name="actions">The auditing actions when property should be audited or empty, if in every action.</param>
        public AuditedPropertyAttribute(bool audited, params AuditingActionType[] actions)
            : this(audited, false, actions)
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="AuditedPropertyAttribute"/> class.
        /// </summary>
        /// <param name="audited"><c>true</c> if property is audited; <c>false</c> otherwise.</param>
        /// <param name="key"><c>true</c> if property is key or part of the composite key; <c>false</c> otherwise.</param>
        /// <param name="actions">The auditing actions when property should be audited or empty, if in every action.</param>
        public AuditedPropertyAttribute(bool audited, bool key, params AuditingActionType[] actions)
        {
            IsAudited = audited;
            IsKeyProperty = key;
            Actions = actions;
        }
        /// <summary>
        /// Gets whether or not property should be audited.
        /// </summary>
        public bool IsAudited { get; }

        /// <summary>
        /// Gets whether or not property is key property.
        /// </summary>
        public bool IsKeyProperty { get; }

        /// <summary>
        /// Gets the auditing actions when property should be audited. 
        /// If empty, then property should be audited in every action.
        /// </summary>
        public IEnumerable<AuditingActionType> Actions { get; } = [];

        /// <summary>
        /// Gets the type of formatter that formats property value to string. 
        /// The format type must implement <see cref="IAuditedPropertyFormatter"/> interface.
        /// </summary>
        public Type? FormatterType { get; set; }
    }
}

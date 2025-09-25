using System.Text.Json.Serialization;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents descriptor of audited property of an audited object instance.
    /// </summary>
    public class AuditedPropertyDescriptor
    {
        /// <summary>
        /// Initializes new instance of the <see cref="AuditedPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="keyProperty"><c>true</c> if property is part of the audited object key; <c>false</c> otherwise.</param>
        /// <exception cref="ArgumentException">If <paramref name="propertyName"/> is empty or only whitespace.</exception>
        public AuditedPropertyDescriptor(string propertyName, string? propertyValue, bool keyProperty)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentException("The property name cannot be empty or whitespace.", nameof(propertyName));

            PropertyName = propertyName;
            PropertyValue = propertyValue;
            IsKeyProperty = keyProperty;
        }

        /// <summary>
        /// Initializes new empty instance of the <see cref="AuditedPropertyDescriptor"/> class.
        /// </summary>
        public AuditedPropertyDescriptor()
        { }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        [JsonIgnore]
        public Guid Identifier { get; internal set; } = Guid.NewGuid();

        /// <summary>
        /// Gets the property name.
        /// </summary>
        [JsonInclude]
        public string PropertyName { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the property value as string or <c>null</c>, if property value is <c>null</c>.
        /// </summary>
        [JsonInclude]
        public string? PropertyValue { get; internal set; }

        /// <summary>
        /// Gets whether or not property is part of the audited object key.
        /// </summary>
        [JsonInclude]
        public bool IsKeyProperty { get; internal set; }
    }
}

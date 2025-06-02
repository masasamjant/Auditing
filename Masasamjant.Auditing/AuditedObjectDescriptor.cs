using System.Text.Json.Serialization;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents descriptor of audited object instance.
    /// </summary>
    public class AuditedObjectDescriptor
    {
        /// <summary>
        /// Initializes new instance of the <see cref="AuditedObjectDescriptor"/> class with specified object key and properties.
        /// </summary>
        /// <param name="key">The audited object key.</param>
        /// <param name="properties">The audited object properties.</param>
        /// <exception cref="ArgumentException">If <paramref name="key"/> represents empty object key.</exception>
        public AuditedObjectDescriptor(AuditedObjectKey key, IEnumerable<AuditedPropertyDescriptor>? properties)
        {
            if (key.IsEmpty)
                throw new ArgumentException("The object key cannot be empty.", nameof(key));

            ObjectKey = key;
            
            if (properties != null && properties.Any())
            {
                Properties.AddRange(properties);
            }
        }

        /// <summary>
        /// Initializes new instance of the <see cref="AuditedObjectDescriptor"/> class with specified object key and properties.
        /// </summary>
        /// <param name="key">The audited object key.</param>
        /// <param name="properties">The audited object properties as name and value pairs.</param>
        /// <exception cref="ArgumentException">If <paramref name="key"/> represents empty object key.</exception>
        public AuditedObjectDescriptor(AuditedObjectKey key, IDictionary<string, string?>? properties)
        {
            if (key.IsEmpty)
                throw new ArgumentException("The object key cannot be empty.", nameof(key));

            ObjectKey = key;
            
            if (properties != null && properties.Count > 0)
            {
                foreach (var keyValue in properties)
                    Properties.Add(new AuditedPropertyDescriptor(keyValue.Key, keyValue.Value));
            }
        }

        /// <summary>
        /// Initialzes new empty instance of the <see cref="AuditedObjectDescriptor"/> class.
        /// </summary>
        public AuditedObjectDescriptor()
        { }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        [JsonIgnore]
        public Guid Identifier { get; internal set; } = Guid.NewGuid();

        /// <summary>
        /// Gets the <see cref="AuditedObjectKey"/> of audited object instance.
        /// </summary>
        [JsonInclude]
        public AuditedObjectKey ObjectKey { get; internal set; } = new AuditedObjectKey();

        /// <summary>
        /// Gets the audited properties.
        /// </summary>
        [JsonInclude]
        public List<AuditedPropertyDescriptor> Properties { get; internal set; } = new List<AuditedPropertyDescriptor>();

        /// <summary>
        /// Gets whether or not this represents empty object descriptor.
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty
        {
            get { return ObjectKey.IsEmpty; }
        }
    }
}

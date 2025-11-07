using System.Text;
using System.Reflection;
using System.Text.Json.Serialization;
using Masasamjant.Auditing.Abstractions;
using Masasamjant.Security;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents key of the audited object instance. Requires that audited object either implements <see cref="IProvideAuditingKeys"/> interface 
    /// or has properties indicated by <see cref="AuditedPropertyAttribute"/> as key properties.
    /// </summary>
    public class AuditedObjectKey : IEquatable<AuditedObjectKey>
    {
        /// <summary>
        /// Initializes new instance of the <see cref="AuditedObjectKey"/> class with specified object instance.
        /// </summary>
        /// <param name="instance">The audited object instance.</param>
        /// <exception cref="ArgumentException">If <paramref name="instance"/> does not have any audited key properties or it does not implement <see cref="IProvideAuditingKeys"/>.</exception>
        public AuditedObjectKey(object instance)
        {
            var key = GetKey(instance);

            if (!key.Any())
                throw new ArgumentException($"The specified instance does not have any audited key properties or does not implement '{typeof(IProvideAuditingKeys)}'.", nameof(instance));

            TypeName = GetTypeName(instance.GetType());
            TypeKey = GetTypeKey(key);
            Value = CreateKeyValue(TypeName, TypeKey);
            DisplayName = instance.GetType().Name;
        }

        /// <summary>
        /// Initializes new empty instance of the <see cref="AuditedObjectKey"/> class.
        /// </summary>
        public AuditedObjectKey()
        { }

        /// <summary>
        /// Initializes new instance of the <see cref="AuditedObjectKey"/> class with specified value and type name.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="typeName">The type name.</param>
        /// <param name="typeKey">The type key.</param>
        /// <param name="displayName">The display name.</param>
        /// <remarks>If value of <paramref name="displayName"/> is <c>null</c>, empty or only whitespace, then uses <paramref name="typeName"/> as display name.</remarks>
        protected AuditedObjectKey(string value, string typeName, string typeKey, string? displayName = null) 
        {
            Value = value;
            TypeName = typeName;
            TypeKey = typeKey;
            DisplayName = string.IsNullOrWhiteSpace(displayName) ? typeName : displayName;
        }

        /// <summary>
        /// Gets the value of the object key.
        /// </summary>
        [JsonInclude]
        public string Value { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the target type name.
        /// </summary>
        [JsonInclude]
        public string TypeName { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the type key as string.
        /// </summary>
        [JsonInclude]
        public string TypeKey { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the display name.
        /// </summary>
        [JsonInclude]
        public string DisplayName { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets whether or not this represents empty object key.
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(Value) || string.IsNullOrWhiteSpace(TypeName); }
        }

        /// <summary>
        /// Check if other audited key is equal to this. Meaning <paramref name="other"/> and <c>this</c> has same value.
        /// </summary>
        /// <param name="other">The other audited key.</param>
        /// <returns><c>true</c> if <paramref name="other"/> has same value as this has; <c>false</c> otherwise.</returns>
        public bool Equals(AuditedObjectKey? other)
        {
            return other != null && string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <summary>
        /// Check if object instance is <see cref="AuditedObjectKey"/> and equal to this.
        /// </summary>
        /// <param name="obj">The object instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is <see cref="AuditedObjectKey"/> and equal to this; <c>false</c> otherwise.</returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as AuditedObjectKey);
        }

        /// <summary>
        /// Gets hash code value.
        /// </summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Gets string representation.
        /// </summary>
        /// <returns>A string representation.</returns>
        public override string ToString()
        {
            var name = string.IsNullOrWhiteSpace(DisplayName) ? TypeName : DisplayName;
            
            if (string.IsNullOrWhiteSpace(name))
                return string.Empty;

            if (string.IsNullOrWhiteSpace(TypeKey))
                return name;

            return $"{name} {TypeKey}";
        }

        private static IEnumerable<object> GetKey(object instance)
        {
            if (instance is IProvideAuditingKeys auditingKeysProvider)
                return auditingKeysProvider.GetAuditingKeys();
            else
            {
                var properties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                
                var keys = new List<object>();
                
                foreach (var property in properties)
                {
                    var keyAttribute = property.GetCustomAttribute<AuditedPropertyAttribute>(false);
                    if (keyAttribute != null && keyAttribute.IsKeyProperty)
                    {
                        var value = property.GetValue(instance, null);

                        if (value != null)
                            keys.Add(value);
                    }
                }

                return keys;
            }
        }

        private static string GetTypeKey(IEnumerable<object> keys)
        {
            var builder = new StringBuilder();
            builder.Append('(');
            foreach (var key in keys)
            {
                var s = key.ToString() ?? string.Empty;
                builder.Append(s).Append(',');
            }
            var value = builder.ToString().TrimEnd(',');
            return value + ")";
        }

        private static string CreateKeyValue(string typeName, string typeKey)
        {
            var provider = new Base64SHA256Provider();
            return provider.CreateHash(typeName + typeKey);
        }

        private static string GetTypeName(Type type)
        {
           return type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
        }
    }
}

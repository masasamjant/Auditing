using System.Text;
using System.Reflection;
using System.Text.Json.Serialization;
using Masasamjant.Auditing.Abstractions;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents key of the audited object instance. Requires that audited object either implements <see cref="IProvideAuditingKeys"/> interface 
    /// or has properties indicated by <see cref="AuditedPropertyAttribute"/> as key properties.
    /// </summary>
    public class AuditedObjectKey
    {
        /// <summary>
        /// Initializes new instance of the <see cref="AuditedObjectKey"/> class with specified object instance.
        /// </summary>
        /// <param name="instance">The audited object instance.</param>
        /// <exception cref="ArgumentException">If <paramref name="instance"/> does not have any key properties for auditing.</exception>
        public AuditedObjectKey(object instance)
        {
            var key = GetKey(instance);

            if (!key.Any())
                throw new ArgumentException("The specified instance does not have any key properties.", nameof(instance));

            TypeName = GetTypeName(instance.GetType());
            Value = CreateKeyValue(TypeName, key);
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
        protected AuditedObjectKey(string value, string typeName) 
        {
            Value = value;
            TypeName = typeName;
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
        /// Gets whether or not this represents empty object key.
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(Value) || string.IsNullOrWhiteSpace(TypeName); }
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

        private static string CreateKeyValue(string name, IEnumerable<object> keys)
        {
            var builder = new StringBuilder(name);
            builder.Append('(');
            
            foreach (var key in keys)
            {
                var s = key.ToString() ?? string.Empty;
                builder.Append(s).Append(',');
            }

            var value = builder.ToString().TrimEnd(',');

            return value + ")";
        }

        private static string GetTypeName(Type type)
        {
           return type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
        }
    }
}

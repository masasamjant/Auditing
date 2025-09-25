using Masasamjant.Auditing.Abstractions;
using System.Reflection;
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

        /// <summary>
        /// Creates <see cref="AuditedObjectDescriptor"/> from specified audited object instance.
        /// </summary>
        /// <param name="instance">The audited object instance.</param>
        /// <param name="action">The <see cref="AuditingActionType"/>.</param>
        /// <returns>A <see cref="AuditedObjectDescriptor"/>.</returns>
        /// <exception cref="NotSupportedException">If any audited property of <paramref name="instance"/> is index property.</exception>
        public static AuditedObjectDescriptor Create(object instance, AuditingActionType action)
        {
            var auditedObjectKey = new AuditedObjectKey(instance);
            var auditedProperties = GetPropertyDescriptors(instance, action);
            return new AuditedObjectDescriptor(auditedObjectKey, auditedProperties);
        }

        private static List<AuditedPropertyDescriptor> GetPropertyDescriptors(object instance, AuditingActionType action)
        {
            var properties = new List<AuditedPropertyDescriptor>();

            foreach (var propertyInfo in GetAuditedPropertyInfos(instance, action)) 
            {
                var descriptor = propertyInfo.CreateDescriptor(instance);
                properties.Add(descriptor);
            }

            return properties;
        }

        private static IEnumerable<AuditedPropertyInfo> GetAuditedPropertyInfos(object instance, AuditingActionType action)
        {
            var properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            var defaultFormatter = new DefaultAuditedPropertyFormatter();
            foreach (var property in properties)
            {
                var indexParameters = property.GetIndexParameters();

                if (indexParameters.Length > 0)
                    throw new NotSupportedException("Index properties are not supported as audited properties.");

                var attribute = property.GetCustomAttribute<AuditedPropertyAttribute>(false);

                if (attribute != null && attribute.IsAudited)
                {
                    var formatter = GetPropertyFormatter(attribute, defaultFormatter);
                    if (attribute.Actions.Any())
                    {
                        if (attribute.Actions.Contains(action))
                            yield return new AuditedPropertyInfo(property, formatter, attribute.IsKeyProperty);
                    }
                    else
                    {
                        yield return new AuditedPropertyInfo(property, defaultFormatter, attribute.IsKeyProperty);
                    }
                }
            }
        }

        private static IAuditedPropertyFormatter GetPropertyFormatter(AuditedPropertyAttribute attribute, IAuditedPropertyFormatter defaultFormatter)
        {
            try
            {
                if (attribute.FormatterType == null)
                    return defaultFormatter;

                var formatter = Activator.CreateInstance(attribute.FormatterType) as IAuditedPropertyFormatter;

                if (formatter != null)
                    return formatter;

                return defaultFormatter;
            }
            catch (Exception)
            {
                return defaultFormatter;
            }
        }

        private class AuditedPropertyInfo
        {
            public AuditedPropertyInfo(PropertyInfo property, IAuditedPropertyFormatter formatter, bool isKeyProperty)
            {
                Property = property;
                Formatter = formatter;
                IsKeyProperty = isKeyProperty;
            }

            public PropertyInfo Property { get; }

            public IAuditedPropertyFormatter Formatter { get; }

            public bool IsKeyProperty { get; }

            public AuditedPropertyDescriptor CreateDescriptor(object instance)
            {
                var value = Property.GetValue(instance, null);

                string? str;

                if (value == null)
                    str = null;
                else if (value is string s)
                    str = s;
                else
                    str = Formatter.Format(instance, Property, value);

                return new AuditedPropertyDescriptor(Property.Name, str, IsKeyProperty);
            }
        }
    }
}

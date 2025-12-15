using Masasamjant.Auditing.Abstractions;
using System.Collections;
using System.Reflection;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents default implementation of <see cref="IAuditedPropertyFormatter"/>.
    /// </summary>
    public sealed class DefaultAuditedPropertyFormatter : IAuditedPropertyFormatter
    {
        private readonly EnumerableAuditedPropertyFormatter enumerableFormatter;

        /// <summary>
        /// Initializes new instance of the <see cref="DefaultAuditedPropertyFormatter"/> class.
        /// </summary>
        public DefaultAuditedPropertyFormatter() 
        {
            enumerableFormatter = new EnumerableAuditedPropertyFormatter();
        }

        /// <summary>
        /// Formats the value of an audited property. This formatter returns the result of <see cref="object.ToString"/>, 
        /// if <paramref name="value"/> is not <c>null</c> or <see cref="IEnumerable"/>. If <paramref name="value"/> is <see cref="IEnumerable"/>, 
        /// then uses <see cref="EnumerableAuditedPropertyFormatter"/>. Otherwise returns <c>null</c>.
        /// </summary>
        /// <param name="instance">The audited object.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> of audited property.</param>
        /// <param name="value">The property value.</param>
        /// <returns>A <paramref name="value"/> formatted to string or <c>null</c>.</returns>
        public string? Format(object instance, PropertyInfo property, object? value)
        {
            if (value == null)
                return null;

            if (value is string s)
                return s;

            if (value is IEnumerable enumerable)
                return enumerableFormatter.Format(enumerable);

            return value.ToString();
        }
    }
}

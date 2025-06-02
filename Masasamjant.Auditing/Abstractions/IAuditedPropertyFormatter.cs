using System.Reflection;

namespace Masasamjant.Auditing.Abstractions
{
    /// <summary>
    /// Represents a component that formats audited property values to string.
    /// </summary>
    public interface IAuditedPropertyFormatter
    {
        /// <summary>
        /// Formats the value of an audited property.
        /// </summary>
        /// <param name="instance">The audited object.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> of audited property.</param>
        /// <param name="value">The property value.</param>
        /// <returns>A <paramref name="value"/> formatted to string or <c>null</c>.</returns>
        string? Format(object instance, PropertyInfo property, object? value);
    }
}

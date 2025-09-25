using Masasamjant.Auditing.Abstractions;
using System.Collections;
using System.Reflection;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents <see cref="IAuditedPropertyFormatter"/> that formats value that implements <see cref="IEnumerable"/> interface.
    /// </summary>
    public sealed class EnumerableAuditedPropertyFormatter : IAuditedPropertyFormatter
    {
        /// <summary>
        /// Default value separator character.
        /// </summary>
        public const char DefaultSeparator = ',';

        /// <summary>
        /// Initializes new default instance of the <see cref="EnumerableAuditedPropertyFormatter"/> class.
        /// </summary>
        public EnumerableAuditedPropertyFormatter()
        {
            Separator = DefaultSeparator;
        }

        /// <summary>
        /// Initializes new instance of the <see cref="EnumerableAuditedPropertyFormatter"/> class.
        /// </summary>
        /// <param name="separator">The value separator character.</param>
        public EnumerableAuditedPropertyFormatter(char separator)
        {
            Separator = separator;
        }

        /// <summary>
        /// Gets the value separator character.
        /// </summary>
        public char Separator { get; }

        /// <summary>
        /// Formats the value of an audited property as concated string, if <paramref name="value"/> is <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="instance">The audited object.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> of audited property.</param>
        /// <param name="value">The property value.</param>
        /// <returns>A <paramref name="value"/> formatted to string or <c>null</c>.</returns>
        public string? Format(object instance, PropertyInfo property, object? value)
        {
            if (value is IEnumerable enumerable)
                return Format(enumerable);

            return null;
        }

        internal string? Format(IEnumerable enumerable) => string.Join(Separator, enumerable);
    }
}

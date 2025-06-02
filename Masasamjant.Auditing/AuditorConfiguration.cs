using Microsoft.Extensions.Configuration;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents auditor configuration.
    /// </summary>
    public class AuditorConfiguration
    {
        /// <summary>
        /// Default value of <see cref="AuditingTimeoutMilliseconds"/>.
        /// </summary>
        public const int DefaultAuditingTimeoutMilliseconds = 5000;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditorConfiguration"/> class that reads configuration 
        /// from the specified <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="auditorSectionName">The name of auditor configuration section.</param>
        public AuditorConfiguration(IConfiguration configuration, string auditorSectionName)
        {
            var section = configuration.GetRequiredSection(auditorSectionName);
            ExcludedTypes = GetExcludedTypes(section);
            AuditingTimeoutMilliseconds = GetAuditingTimeoutMilliseconds(section);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditorConfiguration"/> class with specified values.
        /// </summary>
        /// <param name="excludedTypes">The full name of types that should not be audited.</param>
        /// <param name="auditingTimeoutMilliseconds">The milliseconds timeout for how oftern auditing events are stored.</param>
        public AuditorConfiguration(IEnumerable<string> excludedTypes, double auditingTimeoutMilliseconds)
        {
            ExcludedTypes = excludedTypes;
            AuditingTimeoutMilliseconds = auditingTimeoutMilliseconds > 0D ? auditingTimeoutMilliseconds : DefaultAuditingTimeoutMilliseconds;
        }

        /// <summary>
        /// Gets the full name of types that should not be audited.
        /// </summary>
        public IEnumerable<string> ExcludedTypes { get; }

        /// <summary>
        /// Gets the milliseconds timeout for how oftern auditing events are stored. Default value is 5000 milliseconds.
        /// </summary>
        public double AuditingTimeoutMilliseconds { get; }

        private static string[] GetExcludedTypes(IConfigurationSection section)
        {
            var value = section[nameof(ExcludedTypes)];

            if (string.IsNullOrWhiteSpace(value))
                return [];

            if (value.Contains(','))
            {
                return value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }
            else
            {
                return [value];
            }
        }

        private static double GetAuditingTimeoutMilliseconds(IConfigurationSection section)
        {
            var value = section[nameof(AuditingTimeoutMilliseconds)];
            if (string.IsNullOrWhiteSpace(value))
                return DefaultAuditingTimeoutMilliseconds;
            return double.TryParse(value, out var result) && result > 0D ? result : DefaultAuditingTimeoutMilliseconds;
        }
    }
}

using Microsoft.Extensions.Configuration;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents auditor configuration.
    /// </summary>
    public class AuditorConfiguration
    {
        private readonly IConfiguration configuration;
        private IConfigurationSection? auditorSection;
        private readonly string auditorSectionName;

        /// <summary>
        /// Default value of <see cref="AuditingTimeoutMilliseconds"/>.
        /// </summary>
        public const int DefaultAuditingTimeoutMilliseconds = 5000;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditorConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="auditorSectionName">The name of auditor configuration section.</param>
        public AuditorConfiguration(IConfiguration configuration, string auditorSectionName)
        {
            this.configuration = configuration;
            this.auditorSectionName = auditorSectionName;
        }

        /// <summary>
        /// Gets full name of types that should not be audited.
        /// </summary>
        public IEnumerable<string> ExcludedTypes
        {
            get
            {
                var value = AuditorSection[nameof(ExcludedTypes)];

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
        }

        /// <summary>
        /// Gets milliseconds timeout for how oftern auditing events are stored. Default value is 5000 milliseconds.
        /// </summary>
        public double AuditingTimeoutMilliseconds
        {
            get
            {
                var value = AuditorSection[nameof(AuditingTimeoutMilliseconds)];

                if (string.IsNullOrWhiteSpace(value))
                    return DefaultAuditingTimeoutMilliseconds;

                if (double.TryParse(value, out var result) && result > 0D)
                    return result;

                return DefaultAuditingTimeoutMilliseconds;
            }
        }

        private IConfigurationSection AuditorSection
        {
            get
            {
                if (auditorSection == null)
                    auditorSection = configuration.GetRequiredSection(auditorSectionName);

                return auditorSection;
            }
        }
    }
}

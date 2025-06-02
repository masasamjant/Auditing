using System.Text.Json.Serialization;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents a request to search for auditing events.
    /// </summary>
    public sealed class AuditingEventSearchRequest
    {
        /// <summary>
        /// Gets or sets the application name to filter events by application.
        /// </summary>
        [JsonInclude]
        public string? Application { get; set; }

        /// <summary>
        /// Gets or sets the list of action results to filter events by result.
        /// </summary>
        [JsonInclude]
        public List<AuditingActionResult> Results { get; internal set; } = new List<AuditingActionResult>();

        /// <summary>
        /// Gets or sets the list of action types to filter events by action.
        /// </summary>
        [JsonInclude]
        public List<AuditingActionType> Actions { get; internal set; } = new List<AuditingActionType>();

        /// <summary>
        /// Gets or sets the action name to filter events by action. Ignored if <see cref="Actions"/> is not empty.
        /// </summary>
        [JsonInclude]
        public string? ActionName { get; set; }

        /// <summary>
        /// Gets or sets the start date to filter events by creation date.
        /// </summary>
        [JsonInclude]
        public DateTimeOffset? CreatedFrom { get; set; }

        /// <summary>
        /// Gets or sets the end date to filter events by creation date.
        /// </summary>
        [JsonInclude]
        public DateTimeOffset? CreatedTo { get; set; }

        /// <summary>
        /// Gets or sets the user identifier to filter events by user.
        /// </summary>
        [JsonInclude]
        public string? UserIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the user name to filter events by user.
        /// </summary>
        [JsonInclude]
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of events to return. Default is 2000.
        /// </summary>
        [JsonInclude]
        public int? MaxCount { get; set; } = 2000;

    }
}

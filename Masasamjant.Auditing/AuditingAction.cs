using System.Text.Json.Serialization;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents the audited action.
    /// </summary>
    public class AuditingAction
    {
        /// <summary>
        /// Initializes new instance of the <see cref="AuditingAction"/> class with specified parameters.
        /// </summary>
        /// <param name="applicationName">The application name.</param>
        /// <param name="actionType">The <see cref="AuditingActionType"/>.</param>
        /// <param name="actionResult">The <see cref="AuditingActionResult"/>.</param>
        /// <param name="actionTime">The time when action occurred.</param>
        /// <param name="faultedMessage">The message that descibes why action faulted.</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="applicationName"/> is empty or only whitespace.
        /// -or-
        /// If value of <paramref name="actionType"/> is not defined.
        /// -or-
        /// If value of <paramref name="actionResult"/> is not defined.
        /// </exception>
        public AuditingAction(string applicationName, AuditingActionType actionType, AuditingActionResult actionResult, DateTimeOffset? actionTime = null, string? faultedMessage = null) 
        {
            if (string.IsNullOrWhiteSpace(applicationName))
                throw new ArgumentException("The application name cannot be empty or whitespace.", nameof(applicationName));

            if (!Enum.IsDefined(actionType))
                throw new ArgumentException("The value is not defined.", nameof(actionType));

            if (!Enum.IsDefined(actionResult))
                throw new ArgumentException("The value is not defined.", nameof(actionResult));

            ApplicationName = applicationName;
            ActionName = actionType.ToString();
            ActionType = actionType;
            ActionResult = actionResult;
            ActionTime = actionTime ?? DateTimeOffset.UtcNow;
            FaultedMessage = ActionResult == AuditingActionResult.Faulted ? faultedMessage : null;
        }

        /// <summary>
        /// Initializes new instance of the <see cref="AuditingAction"/> class with specified parameters.
        /// </summary>
        /// <param name="applicationName">The application name.</param>
        /// <param name="actionName">The action name.</param>
        /// <param name="actionResult">The <see cref="AuditingActionResult"/>.</param>
        /// <param name="actionTime">The time when action occurred.</param>
        /// <param name="faultedMessage">The message that descibes why action faulted.</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="applicationName"/> is empty or only whitespace.
        /// -or-
        /// If value of <paramref name="actionType"/> is not defined.
        /// -or-
        /// If value of <paramref name="actionResult"/> is not defined.
        /// </exception>
        public AuditingAction(string applicationName, string actionName, AuditingActionResult actionResult, DateTimeOffset? actionTime = null, string? faultedMessage = null)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
                throw new ArgumentException("The application name cannot be empty or whitespace.", nameof(applicationName));

            if (string.IsNullOrWhiteSpace(actionName))
                throw new ArgumentException("The action name cannot be empty or whitespace.", nameof(actionName));

            if (!Enum.IsDefined(actionResult))
                throw new ArgumentException("The value is not defined.", nameof(actionResult));

            ApplicationName = applicationName;
            ActionName = actionName;
            ActionType = AuditingHelper.GetAuditingActionType(actionName);
            ActionResult = actionResult;
            ActionTime = actionTime ?? DateTimeOffset.UtcNow;
            FaultedMessage = ActionResult == AuditingActionResult.Faulted ? faultedMessage : null;
        }

        /// <summary>
        /// Initializes new empty instance of the <see cref="AuditingAction"/> class.
        /// </summary>
        public AuditingAction()
        { }

        /// <summary>
        /// Gets the name of the application where action was performed.
        /// </summary>
        [JsonInclude]
        public string ApplicationName { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the name of the action that was performed.
        /// </summary>
        [JsonInclude]
        public string ActionName { get; internal set; } = AuditingActionType.Unknown.ToString();

        /// <summary>
        /// Gets the time when action was performed.
        /// </summary>
        [JsonInclude]
        public DateTimeOffset ActionTime { get; internal set; }

        /// <summary>
        /// Gets the type of the action that was performed.
        /// </summary>
        [JsonInclude]
        public AuditingActionType ActionType { get; internal set; }

        /// <summary>
        /// Gets the result of the action that was performed.
        /// </summary>
        [JsonInclude]
        public AuditingActionResult ActionResult { get; internal set; }

        /// <summary>
        /// Gets the message that describes why action faulted, if any.
        /// </summary>
        [JsonInclude]
        public string? FaultedMessage { get; internal set; }
    }
}

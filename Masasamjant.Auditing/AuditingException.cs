namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents an exception that occurs during auditing process.
    /// </summary>
    public class AuditingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditingException"/> class with a default message and an optional auditing event.
        /// </summary>
        /// <param name="auditingEvent">The related auditing event or <c>null</c>.</param>
        public AuditingException(AuditingEvent? auditingEvent)
            : this("An error occurred during auditing.", auditingEvent)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditingException"/> class with a specified message and an optional auditing event.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="auditingEvent">The related auditing event or <c>null</c>.</param>
        public AuditingException(string message, AuditingEvent? auditingEvent)
            : this(message, auditingEvent, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditingException"/> class with a specified message, an optional auditing event, and an inner exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="auditingEvent">The related auditing event or <c>null</c>.</param>
        /// <param name="innerException">The inner exception or <c>null</c>.</param>
        public AuditingException(string message, AuditingEvent? auditingEvent, Exception? innerException)
            : base(message, innerException)
        {
            AuditingEvent = auditingEvent;
        }

        /// <summary>
        /// Gets the related auditing event, if available.
        /// </summary>
        public AuditingEvent? AuditingEvent { get; }
    }
}

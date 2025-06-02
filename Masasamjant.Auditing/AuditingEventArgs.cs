namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents arguments of event to nofity about an auditing event.
    /// </summary>
    public class AuditingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes new instance of the <see cref="AuditingEventArgs"/> class.
        /// </summary>
        /// <param name="auditingEvent">The captured auditing event.</param>
        public AuditingEventArgs(AuditingEvent auditingEvent)
        {
            AuditingEvent = auditingEvent;
        }

        /// <summary>
        /// Gets the captured auditing event.
        /// </summary>
        public AuditingEvent AuditingEvent { get; }
    }
}

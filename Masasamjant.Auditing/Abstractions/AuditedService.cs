namespace Masasamjant.Auditing.Abstractions
{
    /// <summary>
    /// Represents abstract service that implements <see cref="IProvideAuditingEvent"/> interface.
    /// </summary>
    public abstract class AuditedService : IProvideAuditingEvent
    {
        /// <summary>
        /// Notifies when performed action should be audited.
        /// </summary>
        public event AsyncAuditingEventHandler? Audit;

        /// <summary>
        /// Raises <see cref="Audit"/> event.
        /// </summary>
        /// <param name="auditingEvent">The auditing event.</param>
        /// <returns>A task.</returns>
        protected async Task OnAuditAsync(AuditingEvent auditingEvent)
        {
            var args = new AuditingEventArgs(auditingEvent);
            await (Audit?.Invoke(this, args) ?? Task.CompletedTask);
        }
    }
}

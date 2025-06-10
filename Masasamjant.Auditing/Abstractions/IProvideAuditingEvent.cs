namespace Masasamjant.Auditing.Abstractions
{
    /// <summary>
    /// Represents handler of auditing event.
    /// </summary>
    /// <param name="sender">The event source.</param>
    /// <param name="args">The <see cref="AuditingEventArgs"/>.</param>
    /// <returns>A task.</returns>
    public delegate Task AsyncAuditingEventHandler(object? sender, AuditingEventArgs args);

    /// <summary>
    /// Represents component that provides auditing event.
    /// </summary>
    public interface IProvideAuditingEvent
    {
        /// <summary>
        /// Notifies when performed action should be audited.
        /// </summary>
        event AsyncAuditingEventHandler? Audit;
    }
}

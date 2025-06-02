namespace Masasamjant.Auditing.Abstractions
{
    /// <summary>
    /// Represents source to get persisted auditing events.
    /// </summary>
    public interface IAuditingEventSource
    {
        /// <summary>
        /// Gets auditing events based on the specified search request.
        /// </summary>
        /// <param name="request">The <see cref="AuditingEventSearchRequest"/>.</param>
        /// <returns>A found auditing events.</returns>
        Task<IEnumerable<AuditingEvent>> SearchEventsAsync(AuditingEventSearchRequest request);
    }
}

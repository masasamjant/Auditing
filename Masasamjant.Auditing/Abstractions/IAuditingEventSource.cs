namespace Masasamjant.Auditing.Abstractions
{
    /// <summary>
    /// Represents source to get persisted auditing events.
    /// </summary>
    public interface IAuditingEventSource
    {
        /// <summary>
        /// Tries to find auditing event with specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A found auditing event or <c>null</c></returns>
        Task<AuditingEvent?> FindEventAsync(Guid identifier);

        /// <summary>
        /// Search auditing events based on the specified search request.
        /// </summary>
        /// <param name="request">The <see cref="AuditingEventSearchRequest"/>.</param>
        /// <returns>A found auditing events.</returns>
        Task<IEnumerable<AuditingEvent>> SearchEventsAsync(AuditingEventSearchRequest request);
    }
}

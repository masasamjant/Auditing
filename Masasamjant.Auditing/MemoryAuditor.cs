using Masasamjant.Auditing.Abstractions;
using System.Collections.Concurrent;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents an in-memory auditor that stores auditing events in memory.
    /// </summary>
    public sealed class MemoryAuditor : Auditor
    {
        private readonly ConcurrentQueue<AuditingEvent> events;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryAuditor"/> class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The <see cref="AuditorConfiguration"/>.</param>
        public MemoryAuditor(AuditorConfiguration configuration)
            : base(configuration)
        {
            events = new ConcurrentQueue<AuditingEvent>();
        }

        /// <summary>
        /// Tries to find auditing event with specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A found auditing event or <c>null</c></returns>
        public override Task<AuditingEvent?> FindEventAsync(Guid identifier)
        {
            var events = GetAuditingEvents();
            var auditingEvent = events.Where(x => x.Identifier == identifier).FirstOrDefault();
            return Task.FromResult(auditingEvent);
        }

        /// <summary>
        /// Search auditing events based on the specified search request.
        /// </summary>
        /// <param name="request">The <see cref="AuditingEventSearchRequest"/>.</param>
        /// <returns>A found auditing events.</returns>
        public override Task<IEnumerable<AuditingEvent>> SearchEventsAsync(AuditingEventSearchRequest request)
        {
            var events = GetAuditingEvents().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Application))
                events = events.Where(x => x.Action.ApplicationName == request.Application);

            if (request.Results.Count > 0)
                events = events.Where(x => request.Results.Contains(x.Action.ActionResult));

            if (request.Actions.Count > 0)
                events = events.Where(x => request.Actions.Contains(x.Action.ActionType));
            else if (!string.IsNullOrWhiteSpace(request.ActionName))
                events = events.Where(x => x.Action.ActionName == request.ActionName);

            if (request.CreatedFrom.HasValue)
                events = events.Where(x => x.Action.ActionTime >= request.CreatedFrom.Value);

            if (request.CreatedTo.HasValue)
                events = events.Where(x => x.Action.ActionTime <= request.CreatedTo.Value);

            if (!string.IsNullOrWhiteSpace(request.UserIdentifier))
                events = events.Where(x => x.User.UserIdentifier == request.UserIdentifier);

            if (!string.IsNullOrWhiteSpace(request.UserName))
                events = events.Where(x => x.User.UserName == request.UserName);

            if (request.MaxCount > 0)
                events = events.Take(request.MaxCount.Value);

            return Task.FromResult<IEnumerable<AuditingEvent>>(events.ToArray());
        }

        protected override Task OnAuditingAsync(AuditingEvent auditingEvent)
        {
            if (IsDisposed)
                return Task.CompletedTask;

            events.Enqueue(auditingEvent);

            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            base.Dispose(disposing);

            events.Clear();
        }

        private AuditingEvent[] GetAuditingEvents()
        {
            return events.ToArray();
        }
    }
}

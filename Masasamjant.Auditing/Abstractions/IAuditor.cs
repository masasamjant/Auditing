namespace Masasamjant.Auditing.Abstractions
{
    /// <summary>
    /// Represents auditor of auditing events provided by <see cref="IProvideAuditingEvent"/> providers.
    /// </summary>
    public interface IAuditor : IAuditingEventSource
    {
        /// <summary>
        /// Appends specified <see cref="IProvideAuditingEvent"/> to auditor.
        /// </summary>
        /// <param name="source">The <see cref="IProvideAuditingEvent"/> to append.</param>
        void Append(IProvideAuditingEvent source);

        /// <summary>
        /// Removes specified <see cref="IProvideAuditingEvent"/> from auditor.
        /// </summary>
        /// <param name="source">The <see cref="IProvideAuditingEvent"/> to remove.</param>
        void Remove(IProvideAuditingEvent source);

        /// <summary>
        /// Check if specified object is audited.
        /// </summary>
        /// <param name="instance">The object instance.</param>
        /// <returns><c>true</c> if <paramref name="instance"/> is audited; <c>false</c> otherwise.</returns>
        Task<bool> IsAuditedAsync(object instance);
    }
}

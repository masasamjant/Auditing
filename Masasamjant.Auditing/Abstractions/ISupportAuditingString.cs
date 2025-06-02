namespace Masasamjant.Auditing.Abstractions
{
    /// <summary>
    /// Represents object that is audited as value of audited property of another type.
    /// </summary>
    public interface ISupportAuditingString
    {
        /// <summary>
        /// Gets the auditing string.
        /// </summary>
        /// <returns>A auditing string.</returns>
        string GetAuditingString();
    }
}

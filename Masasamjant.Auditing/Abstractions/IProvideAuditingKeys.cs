namespace Masasamjant.Auditing.Abstractions
{
    /// <summary>
    /// Represents a object that can give keys for auditing.
    /// </summary>
    public interface IProvideAuditingKeys
    {
        /// <summary>
        /// Gets array of values of key properties for auditing.
        /// </summary>
        /// <returns>A array of key properties at least with one item.</returns>
        object[] GetAuditingKeys();
    }
}

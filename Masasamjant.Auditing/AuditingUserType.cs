namespace Masasamjant.Auditing
{
    /// <summary>
    /// Defines type of user involved in an auditing event.
    /// </summary>
    public enum AuditingUserType : int
    {
        /// <summary>
        /// Type of user is not defined or unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// User is a human user.
        /// </summary>
        Human = 1,

        /// <summary>
        /// User is an application or service account.
        /// </summary>
        Application = 2
    }
}

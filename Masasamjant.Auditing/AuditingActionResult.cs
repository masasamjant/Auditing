namespace Masasamjant.Auditing
{
    /// <summary>
    /// Defines results of an action that is audited.
    /// </summary>
    public enum AuditingActionResult : int
    {
        /// <summary>
        /// Result of the action is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Action succeeded as expected.
        /// </summary>
        Succeeded = 1,

        /// <summary>
        /// Action failed due to a validation error or business rule violation.
        /// </summary>
        /// <remarks>Like when attempt to insert duplicate information or user authentication failed because invalid cretendials.</remarks>
        Failed = 2,

        /// <summary>
        /// Action faulted because of an exception or unexpected error.
        /// </summary>
        /// <remarks>Like when no connection to database or external system.</remarks>
        Faulted = 3
    }
}

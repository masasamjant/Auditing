namespace Masasamjant.Auditing
{
    /// <summary>
    /// Defines types of auditing event actions that can be audited.
    /// </summary>
    public enum AuditingActionType : int
    {
        /// <summary>
        /// Unknown action.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Information viewed.
        /// </summary>
        View = 1,

        /// <summary>
        /// Information added.
        /// </summary>
        Add = 2,

        /// <summary>
        /// Information updated.
        /// </summary>
        Update = 3,

        /// <summary>
        /// Information removed completely.
        /// </summary>
        Remove = 4,

        /// <summary>
        /// Information imported from external data source.
        /// </summary>
        Import = 5,

        /// <summary>
        /// Information exported to external data source.
        /// </summary>
        Export = 6,

        /// <summary>
        /// Information printed to digital or physical format.
        /// </summary>
        Print = 7,

        /// <summary>
        /// Information merged with other information in same source.
        /// </summary>
        Merge = 8,

        /// <summary>
        /// Authentication of an identity.
        /// </summary>
        Authentication = 9,

        /// <summary>
        /// Authorization of an identity to perform action or access resources.
        /// </summary>
        Authorization = 10,

        /// <summary>
        /// User login to application or system.
        /// </summary>
        Login = 11,

        /// <summary>
        /// User logout from application or system.
        /// </summary>
        Logout = 12,

        /// <summary>
        /// Other actions that do not fit into predefined categories.
        /// </summary>
        Other = 100
    }
}

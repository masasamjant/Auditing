using System.Text.Json.Serialization;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents a user involved in an auditing event.
    /// </summary>
    public class AuditingUser
    {
        private const string AnonumousUserName = "Anonymous";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditingUser"/> class with specified user identifier and user name.
        /// </summary>
        public AuditingUser(string userIdentifier, string userName, AuditingUserType userType)
        {
            if (!Enum.IsDefined(userType))
                throw new ArgumentException("The value is not defined.", nameof(userType));

            UserIdentifier = userIdentifier.Trim();
            UserName = string.IsNullOrEmpty(UserIdentifier) ? AnonumousUserName : userName;
        }

        /// <summary>
        /// Initializes new default instance of the <see cref="AuditingUser"/> class that represents an anonymous user.
        /// </summary>
        public AuditingUser()
        { }

        /// <summary>
        /// Gets the unique user identifier as string.
        /// </summary>
        [JsonInclude]
        public string UserIdentifier { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the user name as string.
        /// </summary>
        [JsonInclude]
        public string UserName { get; internal set; } = AnonumousUserName;

        /// <summary>
        /// Gets the type of the user.
        /// </summary>
        [JsonInclude]
        public AuditingUserType UserType { get; internal set; }
    }
}

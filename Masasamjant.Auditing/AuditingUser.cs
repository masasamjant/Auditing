using System.Text.Json.Serialization;

namespace Masasamjant.Auditing
{
    /// <summary>
    /// Represents a user involved in an auditing event.
    /// </summary>
    public class AuditingUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditingUser"/> class with specified user identifier and user name.
        /// </summary>
        /// <exception cref="ArgumentException">If value of <paramref name="userType"/> is not defined.</exception>
        public AuditingUser(string userIdentifier, string userName, AuditingUserType userType)
        {
            if (!Enum.IsDefined(userType))
                throw new ArgumentException("The value is not defined.", nameof(userType));

            UserIdentifier = userIdentifier;
            UserName = userName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditingUser"/> class with user name.
        /// </summary>
        /// <exception cref="ArgumentException">If value of <paramref name="userType"/> is not defined.</exception>
        public AuditingUser(string userName, AuditingUserType userType)
            : this(string.Empty, userName, userType)
        { }

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
        public string UserName { get; internal set; } = string.Empty;

        /// <summary>
        /// Gets the type of the user.
        /// </summary>
        [JsonInclude]
        public AuditingUserType UserType { get; internal set; }

        /// <summary>
        /// Gets whether or not this represents anonymous user.
        /// </summary>
        /// <remarks><c>true</c> if <see cref="UserIdentifier"/> and <see cref="UserName"/> are both empty or only whitespace; <c>false</c> otherwise.</remarks>
        [JsonIgnore]
        public bool IsAnonymous 
        {
            get { return string.IsNullOrWhiteSpace(UserIdentifier) 
                    && string.IsNullOrWhiteSpace(UserName); }
        }

        /// <summary>
        /// Creates <see cref="AuditingUser"/> that represents anonymous user. This is same as invoking default constructor.
        /// </summary>
        /// <returns>A <see cref="AuditingUser"/> that represents anonymous user.</returns>
        public static AuditingUser AnonymousUser() => new();

        /// <summary>
        /// Creates <see cref="AuditingUser"/> that represents human user.
        /// </summary>
        /// <param name="userIdentifier">The user identifier as string.</param>
        /// <param name="userName">The user name.</param>
        /// <returns>A <see cref="AuditingUser"/> that represents human user.</returns>
        public static AuditingUser HumanUser(string userIdentifier, string userName) => new AuditingUser(userIdentifier, userName, AuditingUserType.Human);

        /// <summary>
        /// Creates <see cref="AuditingUser"/> that represents human user.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A <see cref="AuditingUser"/> that represents human user.</returns>
        public static AuditingUser HumanUser(string userName) => new AuditingUser(userName, AuditingUserType.Human);

        /// <summary>
        /// Creates <see cref="AuditingUser"/> that represents system user.
        /// </summary>
        /// <param name="userIdentifier">The user identifier as string.</param>
        /// <param name="userName">The user name.</param>
        /// <returns>A <see cref="AuditingUser"/> that represents system user.</returns>
        public static AuditingUser SystemUser(string userIdentifier, string userName) => new AuditingUser(userIdentifier, userName, AuditingUserType.System);

        /// <summary>
        /// Creates <see cref="AuditingUser"/> that represents system user.
        /// </summary>
        /// <param name="userIdentifier">The user identifier as string.</param>
        /// <param name="userName">The user name.</param>
        /// <returns>A <see cref="AuditingUser"/> that represents system user.</returns>
        public static AuditingUser SystemUser(string userName) => new AuditingUser(userName, AuditingUserType.System);

        /// <summary>
        /// Creates <see cref="AuditingUser"/> that represents unknown user. This should be case when it's not clear if user is human or system.
        /// </summary>
        /// <param name="userIdentifier">The user identifier as string.</param>
        /// <param name="userName">The user name.</param>
        /// <returns>A <see cref="AuditingUser"/> that represents unknown user.</returns>
        public static AuditingUser UnknownUser(string userIdentifier, string userName) => new AuditingUser(userIdentifier, userName, AuditingUserType.Unknown);

        /// <summary>
        /// Creates <see cref="AuditingUser"/> that represents unknown user. This should be case when it's not clear if user is human or system.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <returns>A <see cref="AuditingUser"/> that represents unknown user.</returns>
        public static AuditingUser UnknownUser(string userName) => new AuditingUser(userName, AuditingUserType.Unknown);
    }
}

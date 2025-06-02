namespace Masasamjant.Auditing
{
    /// <summary>
    /// Provides helper methods for auditing actions.
    /// </summary>
    public static class AuditingHelper
    {
        private static readonly Dictionary<string, AuditingActionType> actions;

        static AuditingHelper()
        {
            actions = new Dictionary<string, AuditingActionType>();

            foreach (var name in Enum.GetNames<AuditingActionType>())
            {
                if (Enum.TryParse(name, out AuditingActionType actionType))
                {
                    actions[name.ToLowerInvariant()] = actionType;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="AuditingActionType"/> based on the action name.
        /// </summary>
        /// <param name="actionName">The action name.</param>
        /// <returns>a <see cref="AuditingActionType"/>.</returns>
        public static AuditingActionType GetAuditingActionType(string actionName)
        {
            if (actions.TryGetValue(actionName.ToLowerInvariant(), out AuditingActionType actionType))
            {
                return actionType;
            }
            else
                return AuditingActionType.Other;
        }
    }
}

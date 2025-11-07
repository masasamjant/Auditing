namespace Masasamjant.Auditing
{
    [TestClass]
    public class AuditingHelperUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_GetAuditingActionType()
        {
            foreach (var value in Enum.GetValues<AuditingActionType>())
            {
                Assert.AreEqual(value, AuditingHelper.GetAuditingActionType(value.ToString()));
            }

            Assert.AreEqual(AuditingActionType.Other, AuditingHelper.GetAuditingActionType(""));
            Assert.AreEqual(AuditingActionType.Other, AuditingHelper.GetAuditingActionType("Custom"));
            Assert.AreEqual(AuditingActionType.Other, AuditingHelper.GetAuditingActionType("Viewing"));
            Assert.AreEqual(AuditingActionType.Other, AuditingHelper.GetAuditingActionType("Insert"));
            Assert.AreEqual(AuditingActionType.Other, AuditingHelper.GetAuditingActionType("Testing"));
            Assert.AreEqual(AuditingActionType.Other, AuditingHelper.GetAuditingActionType("None"));
        }
    }
}

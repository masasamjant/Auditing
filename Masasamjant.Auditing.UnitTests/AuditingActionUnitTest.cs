using System.Text.Json;

namespace Masasamjant.Auditing
{
    [TestClass]
    public class AuditingActionUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Default_Constructor()
        {
            var action = new AuditingAction();
            Assert.AreEqual(string.Empty, action.ApplicationName);
            Assert.AreEqual("Unknown", action.ActionName);
            Assert.AreEqual(default(DateTimeOffset), action.ActionTime);
            Assert.AreEqual(AuditingActionType.Unknown, action.ActionType);
            Assert.AreEqual(AuditingActionResult.Unknown, action.ActionResult);
            Assert.IsNull(action.FaultedMessage);
        }

        [TestMethod]
        public void Test_ActionName_Constructor()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new AuditingAction("", "Update", AuditingActionResult.Succeeded));
            Assert.ThrowsExactly<ArgumentNullException>(() => new AuditingAction("  ", "Update", AuditingActionResult.Succeeded));
            Assert.ThrowsExactly<ArgumentNullException>(() => new AuditingAction("App", "", AuditingActionResult.Succeeded));
            Assert.ThrowsExactly<ArgumentNullException>(() => new AuditingAction("App", "  ", AuditingActionResult.Succeeded));
            Assert.ThrowsExactly<ArgumentException>(() => new AuditingAction("App", "Update", (AuditingActionResult)999));
            var actionTime = DateTimeOffset.UtcNow.AddMinutes(20);

            var action = new AuditingAction("App", "Update", AuditingActionResult.Succeeded, actionTime, "Foo");
            Assert.AreEqual("App", action.ApplicationName);
            Assert.AreEqual("Update", action.ActionName);
            Assert.AreEqual(actionTime, action.ActionTime);
            Assert.AreEqual(AuditingActionType.Update, action.ActionType);
            Assert.AreEqual(AuditingActionResult.Succeeded, action.ActionResult);
            Assert.IsNull(action.FaultedMessage);

            action = new AuditingAction("App", "Foo", AuditingActionResult.Faulted, actionTime, "Bar");
            Assert.AreEqual("App", action.ApplicationName);
            Assert.AreEqual("Foo", action.ActionName);
            Assert.AreEqual(actionTime, action.ActionTime);
            Assert.AreEqual(AuditingActionType.Other, action.ActionType);
            Assert.AreEqual(AuditingActionResult.Faulted, action.ActionResult);
            Assert.AreEqual("Bar", action.FaultedMessage);
        }

        [TestMethod]
        public void Test_ActionType_Constructor()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => new AuditingAction("", AuditingActionType.Update, AuditingActionResult.Succeeded));
            Assert.ThrowsExactly<ArgumentNullException>(() => new AuditingAction("  ", AuditingActionType.Update, AuditingActionResult.Succeeded));
            Assert.ThrowsExactly<ArgumentException>(() => new AuditingAction("App", (AuditingActionType)999, AuditingActionResult.Succeeded));
            Assert.ThrowsExactly<ArgumentException>(() => new AuditingAction("App", AuditingActionType.Update, (AuditingActionResult)999));
            var actionTime = DateTimeOffset.UtcNow.AddMinutes(20);

            var action = new AuditingAction("App", AuditingActionType.Update, AuditingActionResult.Succeeded, actionTime, "Foo");
            Assert.AreEqual("App", action.ApplicationName);
            Assert.AreEqual("Update", action.ActionName);
            Assert.AreEqual(actionTime, action.ActionTime);
            Assert.AreEqual(AuditingActionType.Update, action.ActionType);
            Assert.AreEqual(AuditingActionResult.Succeeded, action.ActionResult);
            Assert.IsNull(action.FaultedMessage);

            action = new AuditingAction("App", AuditingActionType.Update, AuditingActionResult.Faulted, actionTime, "Foo");
            Assert.AreEqual("App", action.ApplicationName);
            Assert.AreEqual("Update", action.ActionName);
            Assert.AreEqual(actionTime, action.ActionTime);
            Assert.AreEqual(AuditingActionType.Update, action.ActionType);
            Assert.AreEqual(AuditingActionResult.Faulted, action.ActionResult);
            Assert.AreEqual("Foo", action.FaultedMessage);
        }

        [TestMethod]
        public void Test_Serialization()
        {
            var actionTime = DateTimeOffset.UtcNow.AddMinutes(20);
            var action = new AuditingAction("App", AuditingActionType.Update, AuditingActionResult.Faulted, actionTime, "Foo");
            var json = JsonSerializer.Serialize(action);
            var other = JsonSerializer.Deserialize<AuditingAction>(json);
            Assert.IsNotNull(other);
            Assert.AreEqual(action.ApplicationName, other.ApplicationName);
            Assert.AreEqual(action.ActionName, other.ActionName);
            Assert.AreEqual(action.ActionTime, other.ActionTime);
            Assert.AreEqual(action.ActionType, other.ActionType);
            Assert.AreEqual(action.ActionResult, other.ActionResult);
            Assert.AreEqual(action.FaultedMessage, other.FaultedMessage);
        }
    }
}

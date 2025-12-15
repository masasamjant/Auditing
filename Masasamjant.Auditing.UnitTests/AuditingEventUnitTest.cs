using System.Text.Json;

namespace Masasamjant.Auditing
{
    [TestClass]
    public class AuditingEventUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Default_Constructor()
        {
            var auditingEvent = new AuditingEvent();
            Assert.AreEqual(Guid.Empty, auditingEvent.Identifier);
            Assert.AreEqual(string.Empty, auditingEvent.User.UserIdentifier);
            Assert.AreEqual(string.Empty, auditingEvent.User.UserName);
            Assert.AreEqual(AuditingUserType.Unknown, auditingEvent.User.UserType);
            Assert.IsTrue(auditingEvent.User.IsAnonymous);
            Assert.IsEmpty(auditingEvent.AuditedObjects);
            Assert.AreEqual(string.Empty, auditingEvent.Action.ApplicationName);
            Assert.AreEqual("Unknown", auditingEvent.Action.ActionName);
            Assert.AreEqual(default(DateTimeOffset), auditingEvent.Action.ActionTime);
            Assert.AreEqual(AuditingActionType.Unknown, auditingEvent.Action.ActionType);
            Assert.AreEqual(AuditingActionResult.Unknown, auditingEvent.Action.ActionResult);
            Assert.IsNull(auditingEvent.Action.FaultedMessage);
        }

        [TestMethod]
        public void Test_Constructor()
        {
            var person = new Person();
            var eventKey = new AuditedObjectKey(person);
            var descriptor = new AuditedObjectDescriptor(eventKey, null);
            var eventUser = new AuditingUser("001", "Test", AuditingUserType.Human);
            var eventAction = new AuditingAction("App", AuditingActionType.Update, AuditingActionResult.Succeeded);
            var auditedObjects = new List<AuditedObjectDescriptor>()
            {
                descriptor
            };
            Assert.ThrowsExactly<ArgumentException>(() => new AuditingEvent(Guid.Empty, eventAction, eventUser, auditedObjects));
            var auditingEvent = new AuditingEvent(Guid.NewGuid(), eventAction, eventUser, auditedObjects);
            Assert.IsTrue(ReferenceEquals(eventAction, auditingEvent.Action));
            Assert.IsTrue(ReferenceEquals(eventUser, auditingEvent.User));
            Assert.IsTrue(auditingEvent.AuditedObjects.Contains(descriptor));
        }

        [TestMethod]
        public void Test_Serialization()
        {
            var person = new Person();
            var eventKey = new AuditedObjectKey(person);
            var descriptor = new AuditedObjectDescriptor(eventKey, null);
            var eventUser = new AuditingUser("001", "Test", AuditingUserType.Human);
            var eventAction = new AuditingAction("App", AuditingActionType.Update, AuditingActionResult.Succeeded);
            var auditedObjects = new List<AuditedObjectDescriptor>()
            {
                descriptor
            };
            var auditingEvent = new AuditingEvent(Guid.NewGuid(), eventAction, eventUser, auditedObjects);
            var json = JsonSerializer.Serialize(auditingEvent);
            var other = JsonSerializer.Deserialize<AuditingEvent>(json);
            Assert.IsNotNull(other);
            Assert.AreEqual(auditingEvent.Identifier, other.Identifier);
            Assert.AreEqual(auditingEvent.User.UserIdentifier, other.User.UserIdentifier);
            Assert.AreEqual(auditingEvent.User.UserName, other.User.UserName);
            Assert.AreEqual(auditingEvent.User.UserType, other.User.UserType);
            Assert.AreEqual(auditingEvent.Action.ApplicationName, other.Action.ApplicationName);
            Assert.AreEqual(auditingEvent.Action.ActionName, other.Action.ActionName);
            Assert.AreEqual(auditingEvent.Action.ActionTime, other.Action.ActionTime);
            Assert.AreEqual(auditingEvent.Action.ActionType, other.Action.ActionType);
            Assert.AreEqual(auditingEvent.Action.ActionResult, other.Action.ActionResult);
            Assert.HasCount(1, other.AuditedObjects);
            Assert.HasCount(auditingEvent.AuditedObjects.Count, other.AuditedObjects);
            Assert.AreEqual(descriptor.ObjectKey, other.AuditedObjects.First().ObjectKey);
        }

        [TestMethod]
        public void Test_CreateForOne()
        {
            var person = new Person();
            var eventKey = new AuditedObjectKey(person);
            var descriptor = new AuditedObjectDescriptor(eventKey, null);
            var eventUser = new AuditingUser("001", "Test", AuditingUserType.Human);
            var eventAction = new AuditingAction("App", AuditingActionType.Update, AuditingActionResult.Succeeded);
            var auditedObjects = new List<AuditedObjectDescriptor>()
            {
                descriptor
            };
            var auditingEvent = new AuditingEvent(Guid.NewGuid(), eventAction, eventUser, auditedObjects);
            
            var other = AuditingEvent.CreateForOne(person, eventAction, eventUser);
            Assert.AreNotEqual(auditingEvent.Identifier, other.Identifier);
            Assert.AreEqual(auditingEvent.User.UserIdentifier, other.User.UserIdentifier);
            Assert.AreEqual(auditingEvent.User.UserName, other.User.UserName);
            Assert.AreEqual(auditingEvent.User.UserType, other.User.UserType);
            Assert.AreEqual(auditingEvent.Action.ApplicationName, other.Action.ApplicationName);
            Assert.AreEqual(auditingEvent.Action.ActionName, other.Action.ActionName);
            Assert.AreEqual(auditingEvent.Action.ActionType, other.Action.ActionType);
            Assert.AreEqual(auditingEvent.Action.ActionResult, other.Action.ActionResult);
            Assert.HasCount(1, other.AuditedObjects);
            Assert.HasCount(auditingEvent.AuditedObjects.Count, other.AuditedObjects);
            Assert.AreEqual(descriptor.ObjectKey, other.AuditedObjects.First().ObjectKey);

            other = AuditingEvent.CreateForOne(person, "App", AuditingActionType.Update, AuditingActionResult.Succeeded, eventUser);
            Assert.AreNotEqual(auditingEvent.Identifier, other.Identifier);
            Assert.AreEqual(auditingEvent.User.UserIdentifier, other.User.UserIdentifier);
            Assert.AreEqual(auditingEvent.User.UserName, other.User.UserName);
            Assert.AreEqual(auditingEvent.User.UserType, other.User.UserType);
            Assert.AreEqual(auditingEvent.Action.ApplicationName, other.Action.ApplicationName);
            Assert.AreEqual(auditingEvent.Action.ActionName, other.Action.ActionName);
            Assert.AreEqual(auditingEvent.Action.ActionType, other.Action.ActionType);
            Assert.AreEqual(auditingEvent.Action.ActionResult, other.Action.ActionResult);
            Assert.HasCount(1, other.AuditedObjects);
            Assert.HasCount(auditingEvent.AuditedObjects.Count, other.AuditedObjects);
            Assert.AreEqual(descriptor.ObjectKey, other.AuditedObjects.First().ObjectKey);
        }

        [TestMethod]
        public void Test_CreateForMany()
        {
            var person = new Person();
            var personKey = new AuditedObjectKey(person);
            var personDescriptor = new AuditedObjectDescriptor(personKey, null);

            var address = new Address();
            var addressKey = new AuditedObjectKey(address);
            var addressDescriptor = new AuditedObjectDescriptor(addressKey, null);

            var eventUser = new AuditingUser("001", "Test", AuditingUserType.Human);
            var eventAction = new AuditingAction("App", AuditingActionType.Update, AuditingActionResult.Succeeded);
            var auditedObjects = new List<AuditedObjectDescriptor>()
            {
                personDescriptor, addressDescriptor
            };

            var auditingEvent = new AuditingEvent(Guid.NewGuid(), eventAction, eventUser, auditedObjects);

            var other = AuditingEvent.CreateForMany(new object[] { person, address }, eventAction, eventUser);
            Assert.AreNotEqual(auditingEvent.Identifier, other.Identifier);
            Assert.AreEqual(auditingEvent.User.UserIdentifier, other.User.UserIdentifier);
            Assert.AreEqual(auditingEvent.User.UserName, other.User.UserName);
            Assert.AreEqual(auditingEvent.User.UserType, other.User.UserType);
            Assert.AreEqual(auditingEvent.Action.ApplicationName, other.Action.ApplicationName);
            Assert.AreEqual(auditingEvent.Action.ActionName, other.Action.ActionName);
            Assert.AreEqual(auditingEvent.Action.ActionType, other.Action.ActionType);
            Assert.AreEqual(auditingEvent.Action.ActionResult, other.Action.ActionResult);
            Assert.HasCount(2, other.AuditedObjects);
            Assert.HasCount(auditingEvent.AuditedObjects.Count, other.AuditedObjects);
            Assert.AreEqual(personDescriptor.ObjectKey, other.AuditedObjects.First().ObjectKey);
            Assert.AreEqual(addressDescriptor.ObjectKey, other.AuditedObjects.Last().ObjectKey);
        }
    }
}

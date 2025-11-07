using System.Text.Json;

namespace Masasamjant.Auditing
{
    [TestClass]
    public class AuditedObjectDescriptorUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Default_Constructor()
        {
            var descriptor = new AuditedObjectDescriptor();
            Assert.IsTrue(descriptor.ObjectKey.IsEmpty);
            Assert.IsTrue(descriptor.IsEmpty);
            Assert.AreEqual(Guid.Empty, descriptor.Identifier);
            Assert.AreEqual(0, descriptor.Properties.Count);
        }

        [TestMethod]
        public void Test_Constructor()
        {
            IEnumerable<AuditedPropertyDescriptor>? properties = null;
            Assert.ThrowsException<ArgumentException>(() => new AuditedObjectDescriptor(new AuditedObjectKey(), properties));
            var person = new Person();
            var key = new AuditedObjectKey(person);
            var descriptor = new AuditedObjectDescriptor(new AuditedObjectKey(person), properties);
            Assert.AreNotEqual(Guid.Empty, descriptor.Identifier);
            Assert.IsFalse(descriptor.IsEmpty);
            Assert.AreEqual(key, descriptor.ObjectKey);
            Assert.AreEqual(0, descriptor.Properties.Count);
            var property = new AuditedPropertyDescriptor(nameof(person.FirstName), person.FirstName, false);
            properties = new List<AuditedPropertyDescriptor>() { property };
            descriptor = new AuditedObjectDescriptor(new AuditedObjectKey(person), properties);
            Assert.AreEqual(1, descriptor.Properties.Count);
            Assert.IsTrue(descriptor.Properties.Contains(property));
        }

        [TestMethod]
        public void Test_Serialization()
        {
            var person = new Person();
            var property = new AuditedPropertyDescriptor(nameof(person.FirstName), person.FirstName, false);
            var properties = new List<AuditedPropertyDescriptor>() { property };
            var descriptor = new AuditedObjectDescriptor(new AuditedObjectKey(person), properties);
            var json = JsonSerializer.Serialize(descriptor);
            var other = JsonSerializer.Deserialize<AuditedObjectDescriptor>(json);
            Assert.IsNotNull(other);
            Assert.AreEqual(descriptor.ObjectKey, other.ObjectKey);
            Assert.AreEqual(descriptor.Properties.Count, other.Properties.Count);
        }

        [TestMethod]
        public void Test_Create()
        {
            var person = new Person();
            var property = new AuditedPropertyDescriptor(nameof(person.Identifier), person.Identifier.ToString(), true);
            var properties = new List<AuditedPropertyDescriptor>() { property };
            var descriptor = new AuditedObjectDescriptor(new AuditedObjectKey(person), properties);
            var other = AuditedObjectDescriptor.Create(person, AuditingActionType.Print);
            Assert.AreEqual(descriptor.ObjectKey, other.ObjectKey);
            Assert.AreEqual(descriptor.Properties.Count, other.Properties.Count);
            Assert.IsTrue(descriptor.Properties.Any(x => x.IsKeyProperty == true));
            Assert.IsTrue(descriptor.Properties.Any(x => x.PropertyName == "Identifier"));
            Assert.IsTrue(descriptor.Properties.Any(x => x.PropertyValue == person.Identifier.ToString()));
            Assert.IsTrue(other.Properties.Any(x => x.IsKeyProperty == true));
            Assert.IsTrue(other.Properties.Any(x => x.PropertyName == "Identifier"));
            Assert.IsTrue(other.Properties.Any(x => x.PropertyValue == person.Identifier.ToString()));
        }
    }
}

using System.Text.Json;

namespace Masasamjant.Auditing
{
    [TestClass]
    public class AuditedObjectKeyUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Protected_Constructor()
        {
            AuditedObjectKey key = new AuditedObjectKeyStub("Value", "Type", "Key");
            Assert.AreEqual("Value", key.Value);
            Assert.AreEqual("Type", key.TypeName);
            Assert.AreEqual("Key", key.TypeKey);
            Assert.AreEqual("Type", key.DisplayName);
            Assert.IsFalse(key.IsEmpty);

            key = new AuditedObjectKeyStub("Value", "Type", "Key", "Display");
            Assert.AreEqual("Value", key.Value);
            Assert.AreEqual("Type", key.TypeName);
            Assert.AreEqual("Key", key.TypeKey);
            Assert.AreEqual("Display", key.DisplayName);
            Assert.IsFalse(key.IsEmpty);
        }

        [TestMethod]
        public void Test_Default_Constructor()
        {
            var key = new AuditedObjectKey();
            Assert.AreEqual(string.Empty, key.Value);
            Assert.AreEqual(string.Empty, key.TypeName);
            Assert.AreEqual(string.Empty, key.DisplayName);
            Assert.IsTrue(key.IsEmpty);
        }

        [TestMethod]
        public void Test_Constructor()
        {
            Assert.ThrowsException<ArgumentException>(() => new AuditedObjectKey(new object()));
            
            var person = new Person();
            var key = new AuditedObjectKey(person);
            Assert.AreEqual($"({person.Identifier})", key.TypeKey);
            Assert.AreEqual(person.GetType().AssemblyQualifiedName, key.TypeName);
            Assert.AreEqual("Person", key.DisplayName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(key.Value));
            Assert.IsFalse(key.IsEmpty);

            var address = new Address();
            key = new AuditedObjectKey(address);
            Assert.AreEqual($"({address.Identifier})", key.TypeKey);
            Assert.AreEqual(address.GetType().AssemblyQualifiedName, key.TypeName);
            Assert.AreEqual("Address", key.DisplayName);
            Assert.IsFalse(string.IsNullOrWhiteSpace(key.Value));
            Assert.IsFalse(key.IsEmpty);
        }

        [TestMethod]
        public void Test_Equals()
        {
            Assert.IsFalse(new AuditedObjectKey().Equals(DateTime.Now));
            Assert.IsTrue(new AuditedObjectKey().Equals(new AuditedObjectKey()));
            var person = new Person();
            var address = new Address();
            Assert.IsTrue(new AuditedObjectKey(person).Equals(new AuditedObjectKey(person)));
            Assert.IsTrue(new AuditedObjectKey(address).Equals(new AuditedObjectKey(address)));
            Assert.IsFalse(new AuditedObjectKey(person).Equals(new AuditedObjectKey(address)));
        }

        [TestMethod]
        public void Test_GetHashCode()
        {
            var person = new Person();
            Assert.AreEqual(new AuditedObjectKey(person).GetHashCode(), new AuditedObjectKey(person).GetHashCode());
            Assert.AreEqual(new AuditedObjectKey().GetHashCode(), new AuditedObjectKey().GetHashCode());
        }

        [TestMethod]
        public void Test_ToString()
        {
            Assert.AreEqual(string.Empty, new AuditedObjectKey().ToString());
            var person = new Person();
            var key = new AuditedObjectKey(person);
            Assert.AreEqual($"Person ({person.Identifier})", key.ToString());
        }

        [TestMethod]
        public void Test_Serialization()
        {
            var person = new Person();
            var key = new AuditedObjectKey(person);
            var json = JsonSerializer.Serialize(key);
            var other = JsonSerializer.Deserialize<AuditedObjectKey>(json);
            Assert.IsNotNull(other);
            Assert.AreEqual(key, other);
        }

        private class AuditedObjectKeyStub : AuditedObjectKey
        {
            public AuditedObjectKeyStub(string value, string typeName, string typeKey, string? displayName = null)
                : base(value, typeName, typeKey, displayName)
            { }
        }
    }
}

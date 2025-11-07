using System.Text.Json;

namespace Masasamjant.Auditing
{
    [TestClass]
    public class AuditedPropertyDescriptorUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Default_Constructor()
        {
            var descriptor = new AuditedPropertyDescriptor();
            Assert.AreEqual(Guid.Empty, descriptor.Identifier);
            Assert.AreEqual(false, descriptor.IsKeyProperty);
            Assert.AreEqual(string.Empty, descriptor.PropertyName);
            Assert.IsNull(descriptor.PropertyValue);
        }

        [TestMethod]
        public void Test_Constructor()
        {
            var descriptor = new AuditedPropertyDescriptor("Name", "Value", true);
            Assert.AreNotEqual(Guid.Empty, descriptor.Identifier);
            Assert.AreEqual(true, descriptor.IsKeyProperty);
            Assert.AreEqual("Name", descriptor.PropertyName);
            Assert.AreEqual("Value", descriptor.PropertyValue);

            descriptor = new AuditedPropertyDescriptor("Name", null, false);
            Assert.AreNotEqual(Guid.Empty, descriptor.Identifier);
            Assert.AreEqual(false, descriptor.IsKeyProperty);
            Assert.AreEqual("Name", descriptor.PropertyName);
            Assert.IsNull(descriptor.PropertyValue);

            Assert.ThrowsException<ArgumentNullException>(() => new AuditedPropertyDescriptor("", null, false));
            Assert.ThrowsException<ArgumentNullException>(() => new AuditedPropertyDescriptor("  ", null, false));
        }

        [TestMethod]
        public void Test_Serialization()
        {
            var descriptor = new AuditedPropertyDescriptor("Name", "Value", true);
            var json = JsonSerializer.Serialize(descriptor);
            var other = JsonSerializer.Deserialize<AuditedPropertyDescriptor>(json);
            Assert.IsNotNull(other);
            Assert.AreEqual(descriptor.PropertyName, other.PropertyName);
            Assert.AreEqual(descriptor.PropertyValue, other.PropertyValue);
            Assert.AreEqual(descriptor.IsKeyProperty, other.IsKeyProperty);
        }
    }
}

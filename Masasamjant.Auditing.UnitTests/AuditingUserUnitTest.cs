using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masasamjant.Auditing
{
    [TestClass]
    public class AuditingUserUnitTest : UnitTest
    {
        [TestMethod]
        public void Test_Default_Constructor()
        {
            var user = new AuditingUser();
            Assert.AreEqual(string.Empty, user.UserName);
            Assert.AreEqual(string.Empty, user.UserIdentifier);
            Assert.AreEqual(AuditingUserType.Unknown, user.UserType);
            Assert.IsTrue(user.IsAnonymous);
        }
    }
}

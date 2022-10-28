using Clippers.Core.Haircut.Models;

namespace Clippers.Test.Unit
{
    [TestClass]
    public class HaircutModelTests
    {
        [TestMethod]
        public void Constructor_ValidConstructorInput_CreatesHaircut()
        {
            var createdAt = DateTime.Now;
            var sut = new HaircutModel("1", "2", "Mr. Bond", createdAt);

            Assert.AreEqual("1", sut.HaircutId);
            Assert.AreEqual("2", sut.CustomerId);
            Assert.AreEqual("Mr. Bond", sut.DisplayName);
            Assert.AreEqual(createdAt, sut.CreatedAt);
            Assert.AreEqual(HaircutStatusType.waiting, sut.HaircutStatus);
        }

    }
}
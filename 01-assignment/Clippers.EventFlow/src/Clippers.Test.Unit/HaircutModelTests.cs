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
            Assert.AreEqual(null, sut.HairdresserId);
        }

        [TestMethod]
        public void Started_ValidInput_StartsHaircut()
        {
            var createdAt = DateTime.Now;
            var startedAt = createdAt.AddMinutes(5);
            var sut = new HaircutModel("1", "2", "Mr. Bond", createdAt);
            sut.Start("3", startedAt);

            Assert.AreEqual("1", sut.HaircutId);
            Assert.AreEqual("2", sut.CustomerId);
            Assert.AreEqual("Mr. Bond", sut.DisplayName);
            Assert.AreEqual(createdAt, sut.CreatedAt);
            Assert.AreEqual(HaircutStatusType.serving, sut.HaircutStatus);
            Assert.AreEqual("3", sut.HairdresserId);
            Assert.AreEqual(startedAt, sut.StartedAt);
        }
    }
}
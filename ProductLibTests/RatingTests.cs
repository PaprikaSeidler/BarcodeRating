using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductLib.Tests
{
    [TestClass()]
    public class RatingTests
    {
        [TestMethod()]
        public void RatingScoreTest()
        {
            Rating rating = new Rating();
            rating.Score = 1;
            Assert.AreEqual(1, rating.Score);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => rating.Score = 0, "Rating must be between 1 and 5.");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => rating.Score = 6, "Rating must be between 1 and 5.");
        }

        [TestMethod()]
        public void RatingDateTest()
        {
            Rating rating = new Rating();
            rating.Date = DateTime.Now;
            Assert.AreEqual(DateTime.Now.Date, rating.Date.Date);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => rating.Date = DateTime.Now.AddDays(1), "Date cannot be in the future.");
        }

        [TestMethod()]
        public void RatingUserTest()
        {
            Rating rating = new Rating();
            rating.User = "TestUser";
            Assert.AreEqual("TestUser", rating.User);
            Assert.ThrowsException<ArgumentNullException>(() => rating.User = null, "User cannot be null or empty.");
            Assert.ThrowsException<ArgumentException>(() => rating.User = "ab", "User must be between 3 and 15 characters long.");
            Assert.ThrowsException<ArgumentException>(() => rating.User = new string('a', 16), "User must be between 3 and 15 characters long.");
        }

        [TestMethod()]
        public void RatingCopyConstructorTest()
        {
            Rating original = new Rating
            {
                Id = 1,
                ProductBarcode = 1234567890,
                Score = 5,
                Comment = "Excellent product!",
                Date = DateTime.Now,
                User = "TestUser"
            };
            Rating copy = new Rating(original);
            Assert.AreEqual(original.Id, copy.Id);
            Assert.AreEqual(original.ProductBarcode, copy.ProductBarcode);
            Assert.AreEqual(original.Score, copy.Score);
            Assert.AreEqual(original.Comment, copy.Comment);
            Assert.AreEqual(original.Date.Date, copy.Date.Date);
            Assert.AreEqual(original.User, copy.User);
        }
    }
}
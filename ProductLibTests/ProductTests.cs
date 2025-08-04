using Microsoft.VisualStudio.TestTools.UnitTesting;
using BarcodeRatingLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeRatingLib.Tests
{
    [TestClass()]
    public class ProductTests
    {
        [TestMethod()]
        public void ProductBarcodeTest()
        {
            Product product = new Product();
            product.Barcode = 1234567890;
            Assert.AreEqual(1234567890, product.Barcode);
            Assert.ThrowsException<ArgumentException>(() => product.Barcode = -1, "Barcode must be a positive integer.");
        }

        [TestMethod()]
        public void ProductCategoryTest()
        {
            Product product = new Product();
            product.Category = "Proteinbar";
            Assert.AreEqual("Proteinbar", product.Category);
            Assert.ThrowsException<ArgumentNullException>(() => product.Category = null, "Category cannot be null.");
            Assert.ThrowsException<ArgumentException>(() => product.Category = "InvalidCategory", "Category must be either 'Proteinbar' or 'Coffee capsule'.");
        }

        [TestMethod()]
        public void ProductCopyConstructorTest()
        {
            Product original = new Product
            {
                Barcode = 1234567890,
                Name = "Test Product",
                ImageUrl = "http://example.com/image.jpg",
                Brand = "Test Brand",
                Category = "Proteinbar"
            };
            Product copy = new Product(original);
            Assert.AreEqual(original.Barcode, copy.Barcode);
            Assert.AreEqual(original.Name, copy.Name);
            Assert.AreEqual(original.ImageUrl, copy.ImageUrl);
            Assert.AreEqual(original.Brand, copy.Brand);
            Assert.AreEqual(original.Category, copy.Category);
        }
    }
}
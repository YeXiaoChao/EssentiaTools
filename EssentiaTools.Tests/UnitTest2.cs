using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EssentiaTools.Models;
using System.Linq;
using Moq;

namespace EssentiaTools.Tests
{
    [TestClass]
    public class UnitTest2
    {
        private Product[] products = {
                                         new Product{Name="Kayak",Catogory="Watersports",Price=275M},
                                         new Product{Name="Lifejacket",Catogory="Watersports",Price=48.95M},
                                         new Product{Name="Soccer ball",Catogory="Soccer",Price=19.50M},
                                         new Product{Name="Corner flag",Catogory="Soccer",Price=34.95M}
                                     };

        [TestMethod]
        public void Sum_Products_Correctly()
        {
            //准备
            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>())).Returns<decimal>(total => total);
            var target = new LinqValueCalculator(mock.Object);

            //动作
            var result = target.ValueProducts(products);

            //断言
            Assert.AreEqual(products.Sum(e => e.Price), result);
        }

        private Product[] createProduct(decimal value)
        {
            return new[] { new Product { Price = value } };
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void Pass_Through_Variable_Discounts()
        {

            Mock<IDiscountHelper> mock = new Mock<IDiscountHelper>();
            mock.Setup(m => m.ApplyDiscount(It.IsAny<decimal>())).Returns<decimal>(total => total);
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v == 0))).Throws<System.ArgumentOutOfRangeException>();
            mock.Setup(m => m.ApplyDiscount(It.Is<decimal>(v => v > 100))).Returns<decimal>(total => (total * 0.9M));
            mock.Setup(m => m.ApplyDiscount(It.IsInRange<decimal>(10, 100, Range.Inclusive))).Returns<decimal>(total => total - 5);
            var target = new LinqValueCalculator(mock.Object);

            decimal FiveDollarDiscount = target.ValueProducts(createProduct(5));
            decimal TenDollarDiscount = target.ValueProducts(createProduct(10));
            decimal FiftyDollarDiscount = target.ValueProducts(createProduct(50));
            decimal HundredDollarDiscount = target.ValueProducts(createProduct(100));
            decimal FiveHundredDollarDiscount = target.ValueProducts(createProduct(500));

            Assert.AreEqual(5, FiveDollarDiscount, "$5 Fail");
            Assert.AreEqual(5, TenDollarDiscount, "$10 Fail");
            Assert.AreEqual(45, FiftyDollarDiscount, "$50 Fail");
            Assert.AreEqual(95, HundredDollarDiscount, "$100 Fail");
            Assert.AreEqual(450, FiveHundredDollarDiscount, "$500 Fail");
            target.ValueProducts(createProduct(0));
        }
    }
}

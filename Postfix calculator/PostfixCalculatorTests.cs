using System;
using NUnit.Framework;

namespace PostfixCalculator
{
    [TestFixture]
    public static class PostfixCalculatorTests
    {
        //В выражении все операторы и числа записываются через пробел.
        [Test]
        public static void UsualOperators()
        {
            var str1 = PostfixCalculator.GetPostfixNotation("( 1 + ( 3 - 4 ) + 5 ) * 6");
            var str2 = PostfixCalculator.GetPostfixNotation("( ( ( 2 - 3 ) * 4 ) + 1 ) / 3");
            var str3 = PostfixCalculator.GetPostfixNotation("1 + 2 ^ ( 1 / 2 )");
            Assert.AreEqual(30, PostfixCalculator.Calculate(str1));
            Assert.AreEqual(-1, PostfixCalculator.Calculate(str2));
            Assert.AreEqual(1 + Math.Sqrt(2), PostfixCalculator.Calculate(str3));
        }

        [Test]
        public static void TrigonometryOperators()
        {
            var str1 = PostfixCalculator.GetPostfixNotation("sin 7 ^ 2 + cos 7 ^ 2");
            var str2 = PostfixCalculator.GetPostfixNotation("sin 3 / cos 3 * ctg 3");
            var str3 = PostfixCalculator.GetPostfixNotation("sin ( 3 + 0. 1415 )");
            var str4 = PostfixCalculator.GetPostfixNotation("2 * sin 3 * cos 3");
            Assert.AreEqual(1,PostfixCalculator.Calculate(str1));
            Assert.AreEqual(1,PostfixCalculator.Calculate(str2));
            Assert.AreEqual(0,PostfixCalculator.Calculate(str3));
            Assert.AreEqual(Math.Sin(6),PostfixCalculator.Calculate(str4));
        }
    }
}

using multiBaseCalc;
using NUnit.Framework;
using System.Windows.Forms;

namespace TestProject
{
    public class DisplayLimitTests
    {
        private Model model;
        private ViewMock view;

        [SetUp]
        public void Setup()
        {
            model = new Model(8);
            view = new ViewMock();
            model.SetView(view);
        }

        [Test]
        public void Basic()
        {
            view.PressKey("12345678=");
            Assert.AreEqual("12345678", view.numberText);
        }

        [Test]
        public void Integer()
        {
            view.PressKey("99999999=");
            Assert.AreEqual("99999999", view.numberText);

            view.PressKey("+1="); //100000000
            Assert.AreEqual("########", view.numberText);

            view.PressKey("/2=");
            Assert.AreEqual("50000000", view.numberText);
        }

        [Test]
        public void Integer_Negative()
        {
            view.PressKey("99999999=");
            Assert.AreEqual("99999999", view.numberText);

            view.PressKey("-==");
            Assert.AreEqual("-99999999", view.numberText);

            view.PressKey("-1="); //-100000000
            Assert.AreEqual("########", view.numberText);

            view.PressKey("/2=");
            Assert.AreEqual("-50000000", view.numberText);
        }

        [Test]
        public void TotalLength_WithFraction_NoOverflow()
        {
            view.PressKey("999999.9=");
            Assert.AreEqual("999999.9", view.numberText);

            view.PressKey("+0.1=");
            Assert.AreEqual("1000000", view.numberText);

            view.PressKey("+0.4=");
            Assert.AreEqual("1000000.4", view.numberText);
        }

        [Test]
        public void TotalLength_WithFraction_Overflow()
        {
            view.PressKey("9999999.9=");
            Assert.AreEqual("9999999.9", view.numberText);

            view.PressKey("+0.1=");
            Assert.AreEqual("10000000", view.numberText);

            view.PressKey("+0.4="); //10000000.4
            Assert.AreEqual("10000000", view.numberText);
        }

        [Test]
        public void Rounding()
        {
            view.PressKey("9999999.9+0.59="); //10000000.49
            Assert.AreEqual("10000000", view.numberText);

            view.PressKey("+0.01="); //10000000.50
            Assert.AreEqual("10000001", view.numberText);

            view.PressKey("+0.01="); //10000000.51
            Assert.AreEqual("10000001", view.numberText);

            view.PressKey("+0.09="); //10000000.60
            Assert.AreEqual("10000001", view.numberText);
        }

        [Test]
        public void Rounding_Negative()
        {
            view.PressKey("-9999999.9-0.59="); //-10000000.49
            Assert.AreEqual("-10000000", view.numberText);

            view.PressKey("-0.01="); //-10000000.50
            Assert.AreEqual("-10000000", view.numberText);

            view.PressKey("-0.01="); //-10000000.51
            Assert.AreEqual("-10000001", view.numberText);

            view.PressKey("-0.09="); //-10000000.60
            Assert.AreEqual("-10000001", view.numberText);
        }

        [Test]
        public void Fraction_RoundingErrors()
        {
            view.PressKey("0.1+=");
            Assert.AreEqual("0.2", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("0.3", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("0.4", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("0.5", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("0.6", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("0.7", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("0.8", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("0.9", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("1", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("1.1", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("1.2", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("1.3", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("1.4", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("1.5", view.numberText);
        }

        [Test]
        public void TrimmingZeros()
        {
            view.PressKey("123.4991+0.000001="); //123.499101
            Assert.AreEqual("123.4991", view.numberText);

            view.PressKey("+0.0009="); //123.500001
            Assert.AreEqual("123.5", view.numberText);
        }

        [Test]
        public void TrimmingPointAndZeros()
        {
            view.PressKey("123456.1=");
            Assert.AreEqual("123456.1", view.numberText);

            view.PressKey("+0.0001="); //123456.1001
            Assert.AreEqual("123456.1", view.numberText);

            view.PressKey("+0.9="); //123457.0001
            Assert.AreEqual("123457", view.numberText);
        }

        [Test]
        public void TrimmingPointAndZeros_DontTrimInteger()
        {
            view.PressKey("123999.7+0.0001="); //123999.7001
            Assert.AreEqual("123999.7", view.numberText);

            view.PressKey("+0.3="); //124000.0001
            Assert.AreEqual("124000", view.numberText);
        }

        [Test]
        public void NegativeFraction()
        {
            view.PressKey("-123999.1111="); //-123999.1111
            Assert.AreEqual("-123999.11", view.numberText);

            view.PressKey("-0.88="); //-123999.9911
            Assert.AreEqual("-123999.99", view.numberText);

            view.PressKey("-0.008="); //-123999.9991
            Assert.AreEqual("-124000", view.numberText);
        }

        [Test]
        public void EnteringLimit()
        {
            view.PressKey("12345678");
            Assert.AreEqual("12345678", view.numberText);

            view.PressKey("9");
            Assert.AreEqual("12345678", view.numberText);

            view.PressKey(Keys.Back);
            Assert.AreEqual("1234567", view.numberText);
        }

        [Test]
        public void EnteringLimit_Fractions()
        {
            view.PressKey("123.45678");
            Assert.AreEqual("123.45678", view.numberText);

            view.PressKey("9");
            Assert.AreEqual("123.45678", view.numberText);

            view.PressKey(Keys.Back);
            Assert.AreEqual("123.4567", view.numberText);
        }

        [Test]
        public void BaseConversion_10And2()
        {
            view.PressKey("255");
            Assert.AreEqual("255", view.numberText);

            view.PressKey("[[[[[[[[");
            Assert.AreEqual("11111111", view.numberText);

            view.PressKey("+1="); //100000000 base 2
            Assert.AreEqual("########", view.numberText);

            view.PressKey("]]]]]]]]");
            Assert.AreEqual("256", view.numberText);
        }

        [Test]
        public void BaseConversion_RoundingBase16()
        {
            view.PressKey("]]]]]]");
            view.PressKey("fffffff.f+0.8f="); //10000000.7f base 16
            Assert.AreEqual("10000000", view.numberText);

            view.PressKey("+0.01="); //10000000.80 base 16
            Assert.AreEqual("10000001", view.numberText);

            view.PressKey("+0.01="); //10000000.81 base 16
            Assert.AreEqual("10000001", view.numberText);

            view.PressKey("+0.0f="); //10000000.90 base 16
            Assert.AreEqual("10000001", view.numberText);
        }

        [Test]
        public void BaseConversion_RoundingBase9()
        {
            view.PressKey("[");
            view.PressKey("4444.4444="); //4444.44440000... base 9
            Assert.AreEqual("4444.4444", view.numberText);

            view.PressKey("+0.00004="); //4444.44444000... base 9
            Assert.AreEqual("4444.4444", view.numberText);

            //note: 1/2 == 0.444... in base 9. So to detect midpoint for rounding,
            //one needs to check if remaining digits
            //are greater than or equal to "444..."
            view.PressKey("+0.00001="); //4444.44445000... base 9
            Assert.AreEqual("4444.4445", view.numberText);

            view.PressKey("-0.00001=+0.000004="); //4444.44444400... base 9
            Assert.AreEqual("4444.4444", view.numberText);

            view.PressKey("+0.000001="); //4444.44444500... base 9
            Assert.AreEqual("4444.4445", view.numberText);

            view.PressKey("1/2="); //0.444... base 9
            Assert.AreEqual("0.4444444", view.numberText);
        }

        [Test]
        public void BaseConversion_RoundingBase9_Negative()
        {
            view.PressKey("[");
            view.PressKey("-4444.4444="); //-4444.44440000... base 9
            Assert.AreEqual("-4444.4444", view.numberText);

            view.PressKey("-0.00004="); //-4444.44444000... base 9
            Assert.AreEqual("-4444.4444", view.numberText);

            view.PressKey("-0.00001="); //-4444.44445000... base 9
            Assert.AreEqual("-4444.4445", view.numberText);

            view.PressKey("+0.00001=-0.000004="); //-4444.44444400... base 9
            Assert.AreEqual("-4444.4444", view.numberText);

            view.PressKey("-0.000001="); //-4444.44444500... base 9
            Assert.AreEqual("-4444.4445", view.numberText);

            view.PressKey(Keys.Escape);
            view.PressKey("-1/2="); //-0.444... base 9 (but assuming finite fraction ...444(000))
            Assert.AreEqual("-0.4444444", view.numberText);
        }
    }
}

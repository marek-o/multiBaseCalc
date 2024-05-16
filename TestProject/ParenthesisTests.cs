using multiBaseCalc;
using NUnit.Framework;

namespace TestProject
{
    public class ParenthesisTests
    {
        private Model model;
        private ViewMock view;

        [SetUp]
        public void Setup()
        {
            model = new Model();
            view = new ViewMock();
            model.SetView(view);
        }

        [Test]
        public void BasicTest()
        {
            view.PressKey("(1+2)");
            Assert.AreEqual("3", view.numberText);

            view.PressKey(Key.Escape);
            view.PressKey("(4+2");
            Assert.AreEqual("2", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("6", view.numberText);

            view.PressKey(Key.Escape);
            view.PressKey("(3+2))))");
            Assert.AreEqual("5", view.numberText);

            view.PressKey(Key.Escape);
            view.PressKey("((((5+2))))");
            Assert.AreEqual("7", view.numberText);
        }

        [Test]
        public void ParensAsFirstNumber()
        {
            view.PressKey("(12+");
            Assert.AreEqual("12", view.numberText);
            view.PressKey("22)");
            Assert.AreEqual("34", view.numberText);
            view.PressKey("/2=");
            Assert.AreEqual("17", view.numberText);
        }

        [Test]
        public void ParensAsSecondNumber()
        {
            view.PressKey("4*(");
            Assert.AreEqual("4", view.numberText);
            view.PressKey("10+");
            Assert.AreEqual("10", view.numberText);
            view.PressKey("20");
            Assert.AreEqual("20", view.numberText);
            view.PressKey(")");
            Assert.AreEqual("30", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("120", view.numberText);
        }

        [Test]
        public void ChainInParens()
        {
            view.PressKey("5*(");
            Assert.AreEqual("5", view.numberText);
            view.PressKey("1+2+3+4+5");
            Assert.AreEqual("5", view.numberText);
            view.PressKey(")");
            Assert.AreEqual("15", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("75", view.numberText);
        }

        [Test]
        public void ChainOutsideParens()
        {
            view.PressKey("1*2*3*4*5");
            Assert.AreEqual("5", view.numberText);
            view.PressKey("*(");
            Assert.AreEqual("120", view.numberText);
            view.PressKey("100+200)");
            Assert.AreEqual("300", view.numberText);
            view.PressKey("*");
            Assert.AreEqual("36000", view.numberText);
            view.PressKey("2=");
            Assert.AreEqual("72000", view.numberText);
        }

        [Test]
        public void ChainInChain()
        {
            view.PressKey("1*2*3*4*5*(");
            Assert.AreEqual("120", view.numberText);
            view.PressKey("1+2+3+4+5+6)");
            Assert.AreEqual("21", view.numberText);
            view.PressKey("+");
            Assert.AreEqual("2520", view.numberText);
            view.PressKey("42=");
            Assert.AreEqual("2562", view.numberText);
        }

        [Test]
        public void Nested()
        {
            /*
               123+45
            -------------
             8 * (41-31)
             */
            view.PressKey("(123+45)");
            Assert.AreEqual("168", view.numberText);
            view.PressKey("/(8*(");
            Assert.AreEqual("8", view.numberText);
            view.PressKey("41-31)");
            Assert.AreEqual("10", view.numberText);
            view.PressKey(")");
            Assert.AreEqual("80", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("2.1", view.numberText);
        }

        [Test]
        public void BrokenInTheMiddle()
        {
            view.PressKey("(123+45)");
            Assert.AreEqual("168", view.numberText);
            view.PressKey("/(8*(");
            Assert.AreEqual("8", view.numberText);
            view.PressKey("42=");
            Assert.AreEqual("0.5", view.numberText);
        }

        [Test]
        public void MultipleNested()
        {
            /*
               1 / (1/125  -  0.003) + 8^2 - sqrt(64)               256
            ------------------------------------------------- =  --------- = -512
                                    3root(100 + (2+3)^2)          2 - 2.5
            log10(2^7 - 28)  -   --------------------------
                                     2*e^(-20 + 5 + 15)
             */
            view.PressKey("((125");
            Assert.AreEqual("125", view.numberText);
            view.PressKey(Key.Inverse);
            Assert.AreEqual("0.008", view.numberText);
            view.PressKey("-0.003)");
            Assert.AreEqual("0.005", view.numberText);
            view.PressKey(Key.Inverse);
            Assert.AreEqual("200", view.numberText);
            view.PressKey("+(8");
            view.PressKey(Key.Square);
            Assert.AreEqual("64", view.numberText);
            view.PressKey(")-(64");
            Assert.AreEqual("64", view.numberText);
            view.PressKey(Key.Sqrt);
            Assert.AreEqual("8", view.numberText);
            view.PressKey("))");
            Assert.AreEqual("256", view.numberText);
            view.PressKey("/(2");
            view.PressKey(Key.Power);
            view.PressKey("7-");
            Assert.AreEqual("128", view.numberText);
            view.PressKey("28");
            view.PressKey(Key.Log10);
            Assert.AreEqual("2", view.numberText);
            view.PressKey("-(100+((2+3)");
            Assert.AreEqual("5", view.numberText);
            view.PressKey(Key.Square);
            Assert.AreEqual("25", view.numberText);
            view.PressKey(")");
            view.PressKey(Key.NthRoot);
            view.PressKey("3/");
            Assert.AreEqual("5", view.numberText);
            view.PressKey("(2*(20");
            view.PressKey(Key.Negate);
            Assert.AreEqual("-20", view.numberText);
            view.PressKey("+5+15");
            view.PressKey(Key.Exp);
            Assert.AreEqual("1", view.numberText);
            view.PressKey("))");
            Assert.AreEqual("2", view.numberText);
            view.PressKey(")");
            Assert.AreEqual("2.5", view.numberText);
            view.PressKey(")");
            Assert.AreEqual("-0.5", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("-512", view.numberText);
        }
    }
}

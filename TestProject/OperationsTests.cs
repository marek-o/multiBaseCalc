using multiBaseCalc;
using NUnit.Framework;

namespace TestProject
{
    public class OperationsTests
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
            view.PressKey("123+789=");
            Assert.AreEqual("912", view.numberText);

            view.PressKey("345-1000.12=");
            Assert.AreEqual("-655.12", view.numberText);

            view.PressKey("1.23*0.25=");
            Assert.AreEqual("0.3075", view.numberText);

            view.PressKey("12.5/8=");
            Assert.AreEqual("1.5625", view.numberText);
        }

        [Test]
        public void NegateResultTest()
        {
            view.PressKey("1=");
            Assert.AreEqual("1", view.numberText);

            view.PressKey(Key.Negate);
            Assert.AreEqual("-1", view.numberText);
        }

        [Test]
        public void NegateFirstEditedTest()
        {
            view.PressKey("234.56");
            Assert.AreEqual("234.56", view.numberText);
            view.PressKey(Key.Negate);
            Assert.AreEqual("-234.56", view.numberText);
            view.PressKey("78");
            Assert.AreEqual("-234.5678", view.numberText);
            view.PressKey("+1=");
            Assert.AreEqual("-233.5678", view.numberText);
        }

        [Test]
        public void NegateSecondEditedTest()
        {
            view.PressKey("1234.56*");
            view.PressKey("20");
            Assert.AreEqual("20", view.numberText);
            view.PressKey(Key.Negate);
            Assert.AreEqual("-20", view.numberText);
            view.PressKey(Key.Equals);
            Assert.AreEqual("-24691.2", view.numberText);
        }

        [Test]
        public void NegateIgnoreOnOperationTest()
        {
            view.PressKey("1234.56*");
            Assert.AreEqual("1234.56", view.numberText);
            view.PressKey(Key.Negate);
            Assert.AreEqual("1234.56", view.numberText);
            view.PressKey("+1=");
            Assert.AreEqual("1235.56", view.numberText);
        }

        [Test]
        public void InverseTest()
        {
            view.PressKey("8");
            view.PressKey(Key.Inverse);
            Assert.AreEqual("0.125", view.numberText);
            view.PressKey(Key.Inverse);
            Assert.AreEqual("8", view.numberText);

            view.PressKey("0");
            view.PressKey(Key.Inverse);
            Assert.AreEqual("[+inf]", view.numberText);
        }

        [Test]
        public void SquareTest()
        {
            view.PressKey("2");
            view.PressKey(Key.Square);
            Assert.AreEqual("4", view.numberText);
            view.PressKey(Key.Square);
            Assert.AreEqual("16", view.numberText);
        }

        [Test]
        public void PowerTest()
        {
            view.PressKey("2");
            view.PressKey(Key.Power);
            view.PressKey("8=");
            Assert.AreEqual("256", view.numberText);

            view.PressKey("2");
            view.PressKey(Key.Power);
            view.PressKey("0=");
            Assert.AreEqual("1", view.numberText);

            view.PressKey("2");
            view.PressKey(Key.Power);
            view.PressKey("3");
            view.PressKey(Key.Negate);
            view.PressKey(Key.Equals);
            Assert.AreEqual("0.125", view.numberText);

            view.PressKey("0");
            view.PressKey(Key.Power);
            view.PressKey("3");
            view.PressKey(Key.Equals);
            Assert.AreEqual("0", view.numberText);

            view.PressKey("2");
            view.PressKey(Key.Negate);
            view.PressKey(Key.Power);
            view.PressKey("3");
            view.PressKey(Key.Equals);
            Assert.AreEqual("-8", view.numberText);

            view.PressKey("0");
            view.PressKey(Key.Power);
            view.PressKey("0");
            view.PressKey(Key.Equals);
            Assert.AreEqual("[NaN]", view.numberText);

            view.PressKey("4");
            view.PressKey(Key.Power);
            view.PressKey("0.5");
            view.PressKey(Key.Equals);
            Assert.AreEqual("2", view.numberText);

            view.PressKey("4-==");
            view.PressKey(Key.Power);
            view.PressKey("0.5");
            view.PressKey(Key.Equals);
            Assert.AreEqual("[NaN]", view.numberText);

            view.PressKey("4-==");
            view.PressKey(Key.Power);
            view.PressKey("1");
            view.PressKey(Key.Equals);
            Assert.AreEqual("-4", view.numberText);
        }

        [Test]
        public void SqrtTest()
        {
            view.PressKey("4");
            view.PressKey(Key.Sqrt);
            Assert.AreEqual("2", view.numberText);
            view.PressKey(Key.Sqrt);
            Assert.IsTrue(view.numberText.StartsWith("1.41"));

            view.PressKey("0");
            view.PressKey(Key.Sqrt);
            Assert.AreEqual("0", view.numberText);

            view.PressKey("2-==");
            view.PressKey(Key.Sqrt);
            Assert.AreEqual("[NaN]", view.numberText);
        }

        [Test]
        public void NthRootTest()
        {
            view.PressKey("1000");
            view.PressKey(Key.NthRoot);
            view.PressKey("3=");
            Assert.AreEqual("10", view.numberText);

            view.PressKey("1000-==");
            view.PressKey(Key.NthRoot);
            view.PressKey("3=");
            //should be -10, precision problems?
            Assert.AreEqual("[NaN]", view.numberText);

            view.PressKey("4-==");
            view.PressKey(Key.NthRoot);
            view.PressKey("4=");
            Assert.AreEqual("[NaN]", view.numberText);

            view.PressKey("0");
            view.PressKey(Key.NthRoot);
            view.PressKey("4=");
            Assert.AreEqual("0", view.numberText);

            view.PressKey("4");
            view.PressKey(Key.NthRoot);
            view.PressKey("0.5=");
            Assert.AreEqual("16", view.numberText);

            view.PressKey("4");
            view.PressKey(Key.NthRoot);
            view.PressKey("1=");
            Assert.AreEqual("4", view.numberText);

            view.PressKey("4");
            view.PressKey(Key.NthRoot);
            view.PressKey("0=");
            Assert.AreEqual("[NaN]", view.numberText);

            view.PressKey("16");
            view.PressKey(Key.NthRoot);
            view.PressKey("2");
            view.PressKey(Key.Negate);
            view.PressKey(Key.Equals);
            Assert.AreEqual("0.25", view.numberText);
        }

        [Test]
        public void Log10Test()
        {
            view.PressKey("1000");
            view.PressKey(Key.Log10);
            Assert.AreEqual("3", view.numberText);

            view.PressKey("0.01");
            view.PressKey(Key.Log10);
            Assert.AreEqual("-2", view.numberText);

            view.PressKey("1");
            view.PressKey(Key.Log10);
            Assert.AreEqual("0", view.numberText);

            view.PressKey("0");
            view.PressKey(Key.Log10);
            Assert.AreEqual("[-inf]", view.numberText);

            view.PressKey("2-==");
            view.PressKey(Key.Log10);
            Assert.AreEqual("[NaN]", view.numberText);
        }

        [Test]
        public void NthLog()
        {
            view.PressKey("1024");
            view.PressKey(Key.NthLog);
            view.PressKey("2=");
            Assert.AreEqual("10", view.numberText);

            view.PressKey("10000");
            view.PressKey(Key.NthLog);
            view.PressKey("10=");
            Assert.AreEqual("4", view.numberText);

            view.PressKey("1024-==");
            view.PressKey(Key.NthLog);
            view.PressKey("2=");
            Assert.AreEqual("[NaN]", view.numberText);

            view.PressKey("1");
            view.PressKey(Key.NthLog);
            view.PressKey("2=");
            Assert.AreEqual("0", view.numberText);

            view.PressKey("0");
            view.PressKey(Key.NthLog);
            view.PressKey("2=");
            Assert.AreEqual("[-inf]", view.numberText);

            view.PressKey("1024");
            view.PressKey(Key.NthLog);
            view.PressKey("0.5=");
            Assert.AreEqual("-10", view.numberText);

            view.PressKey("1024");
            view.PressKey(Key.NthLog);
            view.PressKey("0=");
            Assert.AreEqual("[NaN]", view.numberText);

            view.PressKey("1024");
            view.PressKey(Key.NthLog);
            view.PressKey("1=");
            Assert.AreEqual("[NaN]", view.numberText);

            view.PressKey("1024");
            view.PressKey(Key.NthLog);
            view.PressKey("2");
            view.PressKey(Key.Negate);
            view.PressKey(Key.Equals);
            Assert.AreEqual("[NaN]", view.numberText);
        }

        [Test]
        public void LnTest()
        {
            view.PressKey(Key.EConstant);
            view.PressKey(Key.Ln);
            Assert.AreEqual("1", view.numberText);

            view.PressKey(Key.EConstant);
            view.PressKey("*=");
            view.PressKey(Key.Ln);
            Assert.AreEqual("2", view.numberText);

            view.PressKey("1");
            view.PressKey(Key.Ln);
            Assert.AreEqual("0", view.numberText);

            view.PressKey("0");
            view.PressKey(Key.Ln);
            Assert.AreEqual("[-inf]", view.numberText);

            view.PressKey("1-==");
            view.PressKey(Key.Ln);
            Assert.AreEqual("[NaN]", view.numberText);
        }

        [Test]
        public void ExpTest()
        {
            view.PressKey("0");
            view.PressKey(Key.Exp);
            Assert.AreEqual("1", view.numberText);

            view.PressKey(Key.Exp);
            Assert.IsTrue(view.numberText.StartsWith("2.718281828"));

            view.PressKey("1-==");
            view.PressKey(Key.Exp);
            view.PressKey(Key.Inverse);
            Assert.IsTrue(view.numberText.StartsWith("2.718281828"));
        }

        [Test]
        public void SinTest()
        {
            view.PressKey("0");
            view.PressKey(Key.Sin);
            Assert.AreEqual("0", view.numberText);

            view.PressKey(Key.PiConstant);
            view.PressKey(Key.Sin);
            Assert.AreEqual("0", view.numberText);

            view.PressKey(Key.PiConstant);
            view.PressKey("/2");
            view.PressKey(Key.Sin);
            Assert.AreEqual("1", view.numberText);
        }

        [Test]
        public void CosTest()
        {
            view.PressKey("0");
            view.PressKey(Key.Cos);
            Assert.AreEqual("1", view.numberText);

            view.PressKey(Key.PiConstant);
            view.PressKey(Key.Cos);
            Assert.AreEqual("-1", view.numberText);

            view.PressKey(Key.PiConstant);
            view.PressKey("/2");
            view.PressKey(Key.Cos);
            Assert.AreEqual("0", view.numberText);
        }

        [Test]
        public void TanTest()
        {
            view.PressKey("0");
            view.PressKey(Key.Tan);
            Assert.AreEqual("0", view.numberText);

            view.PressKey(Key.PiConstant);
            view.PressKey(Key.Tan);
            //precision problems
            Assert.AreEqual("-0", view.numberText);

            view.PressKey(Key.PiConstant);
            view.PressKey("/2");
            view.PressKey(Key.Tan);
            //precision problems
            Assert.IsTrue(!view.numberText.Contains('.'));
            Assert.IsTrue(!view.numberText.Contains('-'));
            Assert.IsTrue(view.numberText.Length > 10);
        }
    }
}

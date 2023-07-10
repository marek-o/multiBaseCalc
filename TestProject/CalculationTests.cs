using multiBaseCalc;
using NUnit.Framework;
using System.Windows.Forms;

namespace TestProject
{
    public class CalculationTests
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
        public void InitialState()
        {
            Assert.AreEqual("0", view.numberText);
            Assert.AreEqual("base 10", view.baseLabelText);
        }

        [Test]
        public void Basic()
        {
            Assert.AreEqual("0", view.numberText);
            Assert.AreEqual("base 10", view.baseLabelText);

            view.PressKey("1");
            Assert.AreEqual("1", view.numberText);

            view.PressKey("+");
            Assert.AreEqual("1", view.numberText);

            view.PressKey("2");
            Assert.AreEqual("2", view.numberText);

            view.PressKey(Keys.Enter);
            Assert.AreEqual("3", view.numberText);
        }

        [Test]
        public void Basic2()
        {
            view.PressKey("21/2=");
            Assert.AreEqual("10.5", view.numberText);
        }

        [Test]
        public void DecimalPointEntry()
        {
            view.PressKey("123");
            Assert.AreEqual("123", view.numberText);
            view.PressKey(".");
            Assert.AreEqual("123.", view.numberText);
            view.PressKey("4");
            Assert.AreEqual("123.4", view.numberText);
        }

        [Test]
        public void DecimalPointEntry_Multiple()
        {
            view.PressKey("123");
            Assert.AreEqual("123", view.numberText);
            view.PressKey(".");
            Assert.AreEqual("123.", view.numberText);
            view.PressKey(".");
            Assert.AreEqual("123.", view.numberText);
            view.PressKey("4");
            Assert.AreEqual("123.4", view.numberText);

            view.PressKey(".");
            Assert.AreEqual("123.4", view.numberText);
            view.PressKey("5");
            Assert.AreEqual("123.45", view.numberText);
        }

        [Test]
        public void DecimalPointEntry_Comma()
        {
            view.PressKey("123");
            Assert.AreEqual("123", view.numberText);
            view.PressKey(",");
            Assert.AreEqual("123.", view.numberText);
            view.PressKey("4");
            Assert.AreEqual("123.4", view.numberText);
        }

        [Test]
        public void Editing()
        {
            view.PressKey("123");
            Assert.AreEqual("123", view.numberText);
            view.PressKey(Keys.Back);
            Assert.AreEqual("12", view.numberText);
            view.PressKey("4");
            Assert.AreEqual("124", view.numberText);
            view.PressKey(".567");
            Assert.AreEqual("124.567", view.numberText);
            view.PressKey(Keys.Back);
            view.PressKey(Keys.Back);
            Assert.AreEqual("124.5", view.numberText);
            view.PressKey(Keys.Back);
            Assert.AreEqual("124.", view.numberText);
            view.PressKey(Keys.Back);
            Assert.AreEqual("124", view.numberText);
            view.PressKey(Keys.Back);
            Assert.AreEqual("12", view.numberText);
            view.PressKey(Keys.Back);
            Assert.AreEqual("1", view.numberText);
            view.PressKey(Keys.Back);
            Assert.AreEqual("0", view.numberText);
            view.PressKey(Keys.Back);
            Assert.AreEqual("0", view.numberText);
        }

        [Test]
        public void Editing_SecondNumber()
        {
            view.PressKey("123+");
            Assert.AreEqual("123", view.numberText);
            view.PressKey("4");
            Assert.AreEqual("4", view.numberText);
            view.PressKey(Keys.Back);
            Assert.AreEqual("0", view.numberText);
            view.PressKey(Keys.Back);
            Assert.AreEqual("0", view.numberText);
            view.PressKey("55");
            Assert.AreEqual("55", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("178", view.numberText);
        }

        [Test]
        public void Chain()
        {
            view.PressKey("2*32=");
            Assert.AreEqual("64", view.numberText);
            view.PressKey("*4=");
            Assert.AreEqual("256", view.numberText);
            view.PressKey("+1=");
            Assert.AreEqual("257", view.numberText);
        }

        [Test]
        public void Chain_Break()
        {
            view.PressKey("1000-1=");
            Assert.AreEqual("999", view.numberText);
            view.PressKey("23+45=");
            Assert.AreEqual("68", view.numberText);
            view.PressKey("80/2=");
            Assert.AreEqual("40", view.numberText);
        }
    }
}

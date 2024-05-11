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

            view.PressKey(Key.Add);
            Assert.AreEqual("1", view.numberText);

            view.PressKey("2");
            Assert.AreEqual("2", view.numberText);

            view.PressKey(Key.Equals);
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
            view.PressKey(Key.Backspace);
            Assert.AreEqual("12", view.numberText);
            view.PressKey("4");
            Assert.AreEqual("124", view.numberText);
            view.PressKey(".567");
            Assert.AreEqual("124.567", view.numberText);
            view.PressKey(Key.Backspace);
            view.PressKey(Key.Backspace);
            Assert.AreEqual("124.5", view.numberText);
            view.PressKey(Key.Backspace);
            Assert.AreEqual("124.", view.numberText);
            view.PressKey(Key.Backspace);
            Assert.AreEqual("124", view.numberText);
            view.PressKey(Key.Backspace);
            Assert.AreEqual("12", view.numberText);
            view.PressKey(Key.Backspace);
            Assert.AreEqual("1", view.numberText);
            view.PressKey(Key.Backspace);
            Assert.AreEqual("0", view.numberText);
            view.PressKey(Key.Backspace);
            Assert.AreEqual("0", view.numberText);
        }

        [Test]
        public void Editing_SecondNumber()
        {
            view.PressKey("123+");
            Assert.AreEqual("123", view.numberText);
            view.PressKey("4");
            Assert.AreEqual("4", view.numberText);
            view.PressKey(Key.Backspace);
            Assert.AreEqual("0", view.numberText);
            view.PressKey(Key.Backspace);
            Assert.AreEqual("0", view.numberText);
            view.PressKey("55");
            Assert.AreEqual("55", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("178", view.numberText);
        }

        [Test]
        public void Editing_LeadingZero()
        {
            view.PressKey("000");
            Assert.AreEqual("0", view.numberText);
            view.PressKey("2");
            Assert.AreEqual("2", view.numberText);
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

        [Test]
        public void ChangingOperation()
        {
            view.PressKey("1000");
            Assert.AreEqual("1000", view.numberText);
            view.PressKey("+*25");
            Assert.AreEqual("25", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("25000", view.numberText);
        }

        [Test]
        public void Chain_WithoutEqualsKey()
        {
            view.PressKey("2*32*");
            Assert.AreEqual("64", view.numberText);
            view.PressKey("4+");
            Assert.AreEqual("256", view.numberText);
            view.PressKey("1=");
            Assert.AreEqual("257", view.numberText);
        }

        [Test]
        public void Repeating_Basic()
        {
            view.PressKey("1000+1=");
            Assert.AreEqual("1001", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("1002", view.numberText);
            view.PressKey("=");
            Assert.AreEqual("1003", view.numberText);
        }

        [Test]
        public void Variants()
        {
            view.PressKey("1000+1=");
            Assert.AreEqual("1001", view.numberText);
            view.PressKey("2000=");
            Assert.AreEqual("2001", view.numberText);
            view.PressKey("1234=");
            Assert.AreEqual("1235", view.numberText);
        }

        [Test]
        public void WithItself()
        {
            view.PressKey("1000+=");
            Assert.AreEqual("2000", view.numberText);
            view.PressKey("321-=");
            Assert.AreEqual("0", view.numberText);
            view.PressKey("32*=");
            Assert.AreEqual("1024", view.numberText);
            view.PressKey("444/=");
            Assert.AreEqual("1", view.numberText);
        }

        [Test]
        public void Negating()
        {
            view.PressKey("321-==");
            Assert.AreEqual("-321", view.numberText);
        }

        [Test]
        public void Repeating_Empty()
        {
            view.PressKey("=");
            Assert.AreEqual("0", view.numberText);
        }

        [Test]
        public void Variant_Empty()
        {
            view.PressKey("1234=");
            Assert.AreEqual("1234", view.numberText);
        }

        [Test]
        public void Clearing()
        {
            view.PressKey("123+333=");
            Assert.AreEqual("456", view.numberText);

            view.PressKey(Key.Escape);
            Assert.AreEqual("0", view.numberText);
            view.PressKey("111=");
            Assert.AreEqual("111", view.numberText);

            view.PressKey(Key.Escape);
            Assert.AreEqual("0", view.numberText);
            view.PressKey("+444=");
            Assert.AreEqual("444", view.numberText);
        }

        [Test]
        public void ConstantAsSecondArgument()
        {
            view.PressKey("100+");
            view.PressKey(Key.PiConstant);
            view.PressKey(Key.Equals);

            Assert.AreEqual("103.14159265359", view.numberText);
        }
    }
}

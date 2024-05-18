using multiBaseCalc;
using NUnit.Framework;
using System.Windows.Forms;

namespace TestProject
{
    public class CopyPasteTests
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
        public void Basic()
        {
            view.clipboard = "";
            view.PressKey("1234.5678");
            view.PressKey(Key.Copy);
            Assert.AreEqual("1234.5678", view.clipboard);

            view.PressKey(Key.Escape);
            view.PressKey("55555555");
            view.PressKey(Key.Paste);
            Assert.AreEqual("1234.5678", view.numberText);
        }

        [Test]
        public void PeriodDotAndComma()
        {
            view.clipboard = "123,456";
            view.PressKey(Key.Paste);
            Assert.AreEqual("123.456", view.numberText);

            view.clipboard = "444.333";
            view.PressKey(Key.Paste);
            Assert.AreEqual("444.333", view.numberText);
        }

        [Test]
        public void Negative()
        {
            view.clipboard = "";
            view.PressKey("123.456-==");
            view.PressKey(Key.Copy);
            Assert.AreEqual("-123.456", view.clipboard);

            view.PressKey("123.456");
            view.PressKey(Key.Negate);
            view.PressKey(Key.Copy);
            Assert.AreEqual("-123.456", view.clipboard);

            view.clipboard = "-5.111";
            view.PressKey(Key.Paste);
            Assert.AreEqual("-5.111", view.numberText);
        }

        [Test]
        public void ZeroPeriod()
        {
            view.clipboard = "";
            view.PressKey("0.456");
            view.PressKey(Key.Copy);
            Assert.AreEqual("0.456", view.clipboard);

            view.PressKey(".456");
            view.PressKey(Key.Copy);
            Assert.AreEqual("0.456", view.clipboard);

            view.clipboard = "0.9999";
            view.PressKey(Key.Paste);
            Assert.AreEqual("0.9999", view.numberText);
        }

        [Test]
        public void NegativeZeroPeriod()
        {
            view.clipboard = "";
            view.PressKey("0.456-==");
            view.PressKey(Key.Copy);
            Assert.AreEqual("-0.456", view.clipboard);

            view.PressKey("0.456");
            view.PressKey(Key.Negate);
            view.PressKey(Key.Copy);
            Assert.AreEqual("-0.456", view.clipboard);

            view.clipboard = "-0.9999";
            view.PressKey(Key.Paste);
            Assert.AreEqual("-0.9999", view.numberText);
        }

        [Test]
        public void MultiplePeriods()
        {
            view.clipboard = "111,222.333";
            view.PressKey(Key.Paste);
            Assert.AreEqual("111.222333", view.numberText);
        }

        [Test]
        public void NoZeros()
        {
            view.clipboard = ".1234";
            view.PressKey(Key.Paste);
            Assert.AreEqual("0.1234", view.numberText);
        }

        [Test]
        public void MultipleZeros()
        {
            view.clipboard = "0000.1234";
            view.PressKey(Key.Paste);
            Assert.AreEqual("0.1234", view.numberText);
        }

        [Test]
        public void NegativeMultipleZeros()
        {
            view.clipboard = "-0000.1234";
            view.PressKey(Key.Paste);
            Assert.AreEqual("-0.1234", view.numberText);
        }

        [Test]
        public void MultipleMinuses()
        {
            view.clipboard = "-123-4.12-34";
            view.PressKey(Key.Paste);
            Assert.AreEqual("-1234.1234", view.numberText);
        }

        [Test]
        public void MultipleMinuses2()
        {
            view.clipboard = "123-4-.12-34";
            view.PressKey(Key.Paste);
            Assert.AreEqual("1234.1234", view.numberText);
        }

        [Test]
        public void Whitespace()
        {
            view.clipboard = "   \t11 22 33.  44 55 66\r\n77";
            view.PressKey(Key.Paste);
            Assert.AreEqual("112233.44556677", view.numberText);
        }

        [Test]
        public void RandomCharacters()
        {
            view.clipboard = "32r43T$%Y$Wyv943.4kl'iwa3";
            view.PressKey(Key.Paste);
            Assert.AreEqual("3243943.43", view.numberText);
        }

        [Test]
        public void DigitsOutOfRange()
        {
            view.clipboard = "123A.456B";
            view.PressKey(Key.Paste);
            Assert.AreEqual("123.456", view.numberText);

            view.PressKey(Key.IncrementBase);
            view.PressKey(Key.Paste);
            Assert.AreEqual("123a.456", view.numberText);

            view.PressKey(Key.IncrementBase);
            view.PressKey(Key.Paste);
            Assert.AreEqual("123a.456b", view.numberText);

            view.clipboard = "1204910131.10231";
            view.PressKey(Key.Base2);
            view.PressKey(Key.Paste);
            Assert.AreEqual("101011.101", view.numberText);
        }
    }
}
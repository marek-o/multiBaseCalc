using multiBaseCalc;
using NUnit.Framework;

namespace TestProject
{
    public class BaseConversionTests
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
            view.PressKey("100");
            Assert.AreEqual("100", view.numberText);
            Assert.AreEqual("base 10", view.baseLabelText);

            view.PressKey("]");
            Assert.AreEqual("91", view.numberText);
            Assert.AreEqual("base 11", view.baseLabelText);
        }

        [Test]
        public void Base2To10Test()
        {
            view.PressKey("[[[[[[[[");
            Assert.AreEqual("0", view.numberText);
            Assert.AreEqual("base 2", view.baseLabelText);

            view.PressKey("1011101");
            Assert.AreEqual("1011101", view.numberText);
            Assert.AreEqual("base 2", view.baseLabelText);

            view.PressKey("]]]]]]]]");
            Assert.AreEqual("93", view.numberText);
            Assert.AreEqual("base 10", view.baseLabelText);
        }

        [Test]
        public void Base16To7Test()
        {
            view.PressKey("]]]]]]");
            Assert.AreEqual("0", view.numberText);
            Assert.AreEqual("base 16", view.baseLabelText);

            view.PressKey("8d3");
            Assert.AreEqual("8d3", view.numberText);
            Assert.AreEqual("base 16", view.baseLabelText);

            view.PressKey("[[[[[[[[[");
            Assert.AreEqual("6405", view.numberText);
            Assert.AreEqual("base 7", view.baseLabelText);
        }

        [Test]
        public void Base10To36()
        {
            view.PressKey("12345");
            Assert.AreEqual("12345", view.numberText);
            Assert.AreEqual("base 10", view.baseLabelText);

            view.PressKey(new string(']', 26));
            Assert.AreEqual("9ix", view.numberText);
            Assert.AreEqual("base 36", view.baseLabelText);
        }

        [Test]
        public void BaseWholeRangeTest()
        {
            view.PressKey(new string(']', 26));
            Assert.AreEqual("0", view.numberText);
            Assert.AreEqual("base 36", view.baseLabelText);

            view.PressKey("]");
            Assert.AreEqual("0", view.numberText);
            Assert.AreEqual("base 36", view.baseLabelText);

            view.PressKey("z");

            string[] numbers = new string[]{
                "???", "???", "100011", "1022", "203", "120", "55", "50", "43", "38", //base 0-9
                "35", "32", "2b", "29", "27", "25", "23", "21", "1h", "1g",//base 10-19
                "1f", "1e", "1d", "1c", "1b", "1a", "19", "18", "17", "16",//base 20-29
                "15", "14", "13", "12", "11", "10", "z"//base 30-36
            };

            for (int b = 36; b >= 2; --b)
            {
                Assert.AreEqual(numbers[b], view.numberText);
                Assert.AreEqual(string.Format("base {0}", b), view.baseLabelText);
                view.PressKey("[");
            }
        }
    }
}

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
    }
}

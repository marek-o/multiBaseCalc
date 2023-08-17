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
            model = new Model(3);
            view = new ViewMock();
            model.SetView(view);
        }

        [Test]
        public void Basic()
        {
            Assert.AreEqual("0", view.numberText);
            view.PressKey("123");
            view.PressKey(Keys.Enter);
            Assert.AreEqual("123", view.numberText);
        }
    }
}

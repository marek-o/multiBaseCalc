using multiBaseCalc;
using System;
using System.Windows.Forms;

namespace TestProject
{
    public class ViewMock : IView
    {
        public event Action<char> KeyPressed = delegate { };

        public void PressKey(char key)
        {
            KeyPressed(key);
        }
        
        public void PressKey(Keys key)
        {
            KeyPressed((char)key);
        }

        public void PressKey(string keys)
        {
            foreach (var key in keys)
            {
                PressKey(key);
            }
        }

        public void SetBaseLabel(string s)
        {
            baseLabelText = s;
        }

        public void SetNumber(string s)
        {
            numberText = s;
        }

        public string baseLabelText = "";
        public string numberText = "";
    }
}

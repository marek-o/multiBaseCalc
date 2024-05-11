using multiBaseCalc;
using System;
using System.Collections.Generic;

namespace TestProject
{
    public class ViewMock : IView
    {
        public event Action<Key> KeyPressed = delegate { };

        public void PressKey(char ch)
        {
            if (!Form1.KeyTranslation.TryGetValue(ch, out Key key))
            {
                throw new ArgumentException(string.Format("character not recognized: {0}", ch));
            }

            KeyPressed(key);
        }

        public void PressKey(Key key)
        {
            KeyPressed(key);
        }

        public void PressKey(string keys)
        {
            foreach (var key in keys)
            {
                PressKey(key);
            }
        }

        public void PressKey(IEnumerable<Key> keys)
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

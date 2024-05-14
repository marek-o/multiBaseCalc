using multiBaseCalc;
using System;
using System.Collections.Generic;

namespace TestProject
{
    public class ViewMock : IView
    {
        public event Action<Key> KeyPressed = delegate { };

        public Dictionary<char, Key> CharToKey = new Dictionary<char, Key>(
            new KeyValuePair<char, Key>[]{
                new KeyValuePair<char, Key>('0', Key.D0),
                new KeyValuePair<char, Key>('1', Key.D1),
                new KeyValuePair<char, Key>('2', Key.D2),
                new KeyValuePair<char, Key>('3', Key.D3),
                new KeyValuePair<char, Key>('4', Key.D4),
                new KeyValuePair<char, Key>('5', Key.D5),
                new KeyValuePair<char, Key>('6', Key.D6),
                new KeyValuePair<char, Key>('7', Key.D7),
                new KeyValuePair<char, Key>('8', Key.D8),
                new KeyValuePair<char, Key>('9', Key.D9),
                new KeyValuePair<char, Key>('a', Key.A),
                new KeyValuePair<char, Key>('b', Key.B),
                new KeyValuePair<char, Key>('c', Key.C),
                new KeyValuePair<char, Key>('d', Key.D),
                new KeyValuePair<char, Key>('e', Key.E),
                new KeyValuePair<char, Key>('f', Key.F),
                new KeyValuePair<char, Key>('g', Key.G),
                new KeyValuePair<char, Key>('h', Key.H),
                new KeyValuePair<char, Key>('i', Key.I),
                new KeyValuePair<char, Key>('j', Key.J),
                new KeyValuePair<char, Key>('k', Key.K),
                new KeyValuePair<char, Key>('l', Key.L),
                new KeyValuePair<char, Key>('m', Key.M),
                new KeyValuePair<char, Key>('n', Key.N),
                new KeyValuePair<char, Key>('o', Key.O),
                new KeyValuePair<char, Key>('p', Key.P),
                new KeyValuePair<char, Key>('q', Key.Q),
                new KeyValuePair<char, Key>('r', Key.R),
                new KeyValuePair<char, Key>('s', Key.S),
                new KeyValuePair<char, Key>('t', Key.T),
                new KeyValuePair<char, Key>('u', Key.U),
                new KeyValuePair<char, Key>('v', Key.V),
                new KeyValuePair<char, Key>('w', Key.W),
                new KeyValuePair<char, Key>('x', Key.X),
                new KeyValuePair<char, Key>('y', Key.Y),
                new KeyValuePair<char, Key>('z', Key.Z),
                new KeyValuePair<char, Key>(',', Key.Period),
                new KeyValuePair<char, Key>('.', Key.Period),
                new KeyValuePair<char, Key>('=', Key.Equals),
                new KeyValuePair<char, Key>('+', Key.Add),
                new KeyValuePair<char, Key>('-', Key.Subtract),
                new KeyValuePair<char, Key>('*', Key.Multiply),
                new KeyValuePair<char, Key>('/', Key.Divide),
                new KeyValuePair<char, Key>('(', Key.ParenOpen),
                new KeyValuePair<char, Key>(')', Key.ParenClose),
            }
            );

        public void PressKey(char ch)
        {
            if (!CharToKey.TryGetValue(ch, out Key key))
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

        public void SetNumber(string s, int separatorGroupSize)
        {
            numberText = s;
        }

        public string baseLabelText = "";
        public string numberText = "";
    }
}

using NUnit.Framework;
using multiBaseCalc;

namespace TestProject
{
    public class BaseConverterTests
    {
        [SetUp]
        public void Setup()
        {
        }


        [TestCase("", 0.0)]
        [TestCase("0", 0.0)]
        [TestCase("0.00", 0.0)]
        [TestCase("0,00", 0.0)]
        [TestCase("-0", 0.0)]
        [TestCase("-0.0", 0.0)]
        [TestCase("-0,0", 0.0)]

        [TestCase("1", 1.0)]
        [TestCase("-1", -1.0)]
        [TestCase("100.0", 100.0)]
        [TestCase("1234,0", 1234.0)]
        [TestCase("-444.00", -444.0)]
        [TestCase("999999", 999999.0)]
        [TestCase("-999999", -999999.0)]
        [TestCase("123456789012345", 123456789012345.0)]
        [TestCase("-123456789012345", -123456789012345.0)]

        [TestCase("1234.5678", 1234.5678)]
        [TestCase("-1234.5678", -1234.5678)]
        [TestCase("-1234,5678", -1234.5678)]
        [TestCase("0.999999", 0.999999)]
        [TestCase("-0.999999", -0.999999)]
        [TestCase(".999999", 0.999999)]
        [TestCase("-.999999", -0.999999)]

        [TestCase("3.1415926535897", 3.1415926535897)]
        [TestCase("1000000000000000", 1e15)]
        [TestCase("-5000000000000000", -5e15)]
        [TestCase("1000000000000000000000000000000", 1e30, 1e15)]
        [TestCase("0.001", 1e-3)]
        [TestCase("0.0000000000000001", 1e-16)]
        [TestCase("-0.0000000000000000000000000000001", -1e-31)]
        [TestCase("-0.00000000000000000000000000000012345", -1.2345e-31)]
        public void TestStringToDoubleBase10(string input, double expected,
            double epsilon = 0.0)
        {
            Assert.AreEqual(expected, BaseConverter.StringToDouble(input, 10), epsilon);
        }

        [TestCase("0", 0.0)]
        [TestCase("0", -0.0)]

        [TestCase("1", 1.0)]
        [TestCase("-1", -1.0)]
        [TestCase("100", 100.0)]
        [TestCase("1234", 1234.0)]
        [TestCase("-444", -444.0)]
        [TestCase("999999", 999999.0)]
        [TestCase("-999999", -999999.0)]
        [TestCase("123456789012345", 123456789012345.0)]
        [TestCase("-123456789012345", -123456789012345.0)]

        [TestCase("1234.5678", 1234.5678, 16)]
        [TestCase("-1234.5678", -1234.5678, 16)]
        [TestCase("0.999999", 0.999999, 16)]
        [TestCase("-0.999999", -0.999999, 16)]

        [TestCase("3.1415926535897", 3.1415926535897, 16)]
        [TestCase("1000000000000000", 1e15)]
        [TestCase("-5000000000000000", -5e15)]
        [TestCase("0.001", 1e-3)]
        [TestCase("0.0000000000000001", 1e-16, 31)]
        [TestCase("-0.0000000000000000000000000000001", -1e-31, 46)]
        [TestCase("-0.00000000000000000000000000000012345", -1.2345e-31, 46)]

        [TestCase("[+inf]", double.PositiveInfinity)]
        [TestCase("[-inf]", double.NegativeInfinity)]
        [TestCase("[NaN]", double.NaN)]
        public void TestDoubleToStringBase10(string expected, double input,
            int maxDigitCount = 50)
        {
            Assert.AreEqual(expected,
                BaseConverter.DoubleToString(input, 10, maxDigitCount));
        }

        [TestCase("", 0.0)]
        [TestCase("0", 0.0)]
        [TestCase("0.00", 0.0)]
        [TestCase("0,00", 0.0)]
        [TestCase("-0", 0.0)]
        [TestCase("-0.0", 0.0)]
        [TestCase("-0,0", 0.0)]

        [TestCase("1", 1.0)]
        [TestCase("-1", -1.0)]
        [TestCase("100.0", 4.0)]
        [TestCase("11010,0", 26.0)]
        [TestCase("-110111.00", -55.0)]
        [TestCase("11110100001000111111", 999999.0)]
        [TestCase("-11110100001000111111", -999999.0)]
        [TestCase("011100000100100010000110000011011101111101111001", 123456789012345.0)]
        [TestCase("-011100000100100010000110000011011101111101111001", -123456789012345.0)]

        [TestCase("1010.0011", 10.1875)]
        [TestCase("-1010.0011", -10.1875)]
        [TestCase("-1010,0011", -10.1875)]
        [TestCase("0.111111", 0.984375)]
        [TestCase("-0.111111", -0.984375)]
        [TestCase(".111111", 0.984375)]
        [TestCase("-.111111", -0.984375)]

        [TestCase("11.0010010000111111", 3.1415863037109375)]
        [TestCase("1000000000000000", 32768.0)]
        [TestCase("-101000000000000000", -163840.0)]
        [TestCase("1000000000000000000000000000000", 1073741824.0)]
        [TestCase("1000000000000000000000000000000000000000000000000000000000000", 1152921504606846976.0)]
        [TestCase("0.001", 0.125)]
        [TestCase("0.0000000000000001", 0.0000152587890625)]
        [TestCase("-0.0000000000000000000000000000001", -0.0000000004656612873077392578125)]
        [TestCase("-0.000000000000000000000000000000101", -5.82076609134674072265625e-10)]
        public void TestStringToDoubleBase2(string s, double expected)
        {
            Assert.AreEqual(expected, BaseConverter.StringToDouble(s, 2));
        }

        [TestCase("", 0.0)]
        [TestCase("0", 0.0)]
        [TestCase("0.00", 0.0)]

        [TestCase("1", 1.0)]
        [TestCase("-1", -1.0)]
        [TestCase("100.0", 256.0)]
        [TestCase("c1,0", 193.0)]
        [TestCase("-1f.00", -31.0)]

        [TestCase("c.4", 12.25)]
        [TestCase("-f.fa", -15.9765625)]

        [TestCase("3.243F6A8822E87C", 3.1415926535)]
        [TestCase("0.001", 0.000244140625)]
        public void TestStringToDoubleBase16(string s, double expected)
        {
            Assert.AreEqual(expected, BaseConverter.StringToDouble(s, 16));
        }
    }
}
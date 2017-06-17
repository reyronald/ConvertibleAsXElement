using AsXElement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConvertibleToXElementTests
{
    [TestClass]
    public class ConvertibleToXElement_Tests_For_ValueTypes
    {
        private class AClassWithValueTypeProperties : ConvertibleToXElement
        {
            public bool SomeBool { get; set; } = true;
            public bool? SomeBoolNullable { get; set; } = null;

            public double SomeDouble { get; set; } = 10.50d;
            public double? SomeDoubleNullable { get; set; } = null;

            public int SomeInt { get; set; }
            public int? SomeIntNullable { get; set; } = null;

            public string SomeString { get; set; } = "Some string value";
            public string SomeStringEmpty { get; set; } = string.Empty;
            public string SomeStringNulalble { get; set; } = null;

            [NonConvertibleToXElement]
            public string SomeNotMappedString { get; set; }
        }

        [TestMethod]
        public void Serialization_Should_Be_Correct_Without_Namespace()
        {
            var instance = new AClassWithValueTypeProperties();

            string actual = instance.AsXElement().ToString().Trim();

            string expected = @"
<AClassWithValueTypeProperties>
  <SomeBool>true</SomeBool>
  <SomeBoolNullable />
  <SomeDouble>10.5</SomeDouble>
  <SomeDoubleNullable />
  <SomeInt>0</SomeInt>
  <SomeIntNullable />
  <SomeString>Some string value</SomeString>
  <SomeStringEmpty></SomeStringEmpty>
  <SomeStringNulalble />
</AClassWithValueTypeProperties>".Trim();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Serialization_Should_Be_Correct_With_Namespace()
        {
            var instance = new AClassWithValueTypeProperties();
            const string xnamespace = "http://somenamespace.com";
            string actual = instance.AsXElement(xnamespace).ToString().Trim();

            string expected = @"
<AClassWithValueTypeProperties xmlns=""http://somenamespace.com"">
  <SomeBool>true</SomeBool>
  <SomeBoolNullable />
  <SomeDouble>10.5</SomeDouble>
  <SomeDoubleNullable />
  <SomeInt>0</SomeInt>
  <SomeIntNullable />
  <SomeString>Some string value</SomeString>
  <SomeStringEmpty></SomeStringEmpty>
  <SomeStringNulalble />
</AClassWithValueTypeProperties>".Trim();

            Assert.AreEqual(expected, actual);
        }
    }
}
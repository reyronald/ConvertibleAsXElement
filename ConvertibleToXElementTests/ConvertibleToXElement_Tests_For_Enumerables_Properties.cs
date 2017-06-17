using AsXElement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ConvertibleToXElementTests
{
    [TestClass]
    public class ConvertibleToXElement_Tests_For_Enumerables_Properties
    {
        private class SomeClassWithValueTypeEnumerableProperties : ConvertibleToXElement
        {
            public int Id { get; set; }
            public string Name { get; set; } = "John Smith";
            public IEnumerable<string> Children { get; set; } = new List<string>
            {
                "Jane Smith",
                "Jacob Smith"
            };
        }

        [TestMethod]
        public void Serialization_Should_Be_Correct_For_ValueType_Enumerable()
        {
            var instance = new SomeClassWithValueTypeEnumerableProperties();

            string actual = instance.AsXElement().ToString().Trim();

            string expected = @"
<SomeClassWithValueTypeEnumerableProperties>
  <Id>0</Id>
  <Name>John Smith</Name>
  <Children>
    <String>Jane Smith</String>
    <String>Jacob Smith</String>
  </Children>
</SomeClassWithValueTypeEnumerableProperties>".Trim();

            Assert.AreEqual(expected, actual);
        }

        private class SomeClassWithComplexTypeEnumerableProperties : ConvertibleToXElement
        {
            public class Property : ConvertibleToXElement
            {
                public string Name { get; set; } = "Name";
                public string Value { get; set; } = "Value";
            }

            public IEnumerable<Property> AllProperties { get; set; } = new List<Property>
            {
                new Property(),
                new Property(),
            };
        }

        [TestMethod]
        public void Serialization_Should_Be_Correct_For_ComplexType_Enumerable()
        {
            var instance = new SomeClassWithComplexTypeEnumerableProperties();

            string actual = instance.AsXElement().ToString().Trim();

            string expected = @"
<SomeClassWithComplexTypeEnumerableProperties>
  <AllProperties>
    <Property>
      <Name>Name</Name>
      <Value>Value</Value>
    </Property>
    <Property>
      <Name>Name</Name>
      <Value>Value</Value>
    </Property>
  </AllProperties>
</SomeClassWithComplexTypeEnumerableProperties>".Trim();

            Assert.AreEqual(expected, actual);
        }
    }
}
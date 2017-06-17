using AsXElement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace onvertibleToXElementTests
{
    [TestClass]
    public class ConvertibleToXElement_Tests_For_ConvertibleToXElement_ComplexTypes
    {
        private class SomeClassWithComplexTypesProperties : ConvertibleToXElement
        {
            public class SomeConvertibleToXElementInnerClass : ConvertibleToXElement
            {
                public string Name { get; set; } = "John Smith";
                [NonConvertibleToXElement]
                public int NotMappedIntProperty { get; set; }
            }

            public int Id { get; set; } = 1;
            [NonConvertibleToXElement]
            public string NotMappedStringProperty { get; set; }
            public SomeConvertibleToXElementInnerClass ComplexType { get; set; }
        }

        [TestMethod]
        public void Serialization_Should_Be_Correct_For_Null_ComplexType_Property()
        {
            var instance = new SomeClassWithComplexTypesProperties
            {
                ComplexType = new SomeClassWithComplexTypesProperties.SomeConvertibleToXElementInnerClass()
            };

            string actual = instance.AsXElement().ToString().Trim();

            string expected = @"
<SomeClassWithComplexTypesProperties>
  <Id>1</Id>
  <SomeConvertibleToXElementInnerClass>
    <Name>John Smith</Name>
  </SomeConvertibleToXElementInnerClass>
</SomeClassWithComplexTypesProperties>".Trim();

            Assert.AreEqual(expected, actual);
        }
    }
}
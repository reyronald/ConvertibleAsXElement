using AsXElement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProdoctivityOpenXMLModel.Tests.ConvertibleToXElementTests
{
    [TestClass]
    public class ConvertibleToXElement_Tests_For_ComplexTypes
    {
        private class SomeClassWithComplexTypesProperties : ConvertibleToXElement
        {
            public class SomeClass
            {
                public string Name { get; set; } = "John Smith";
                [NonConvertibleToXElement]
                public int NotMappedIntProperty { get; set; }
            }

            public int Id { get; set; } = 1;
            [NonConvertibleToXElement]
            public string NotMappedStringProperty { get; set; }
            public SomeClass ComplexType { get; set; }
        }

        [TestMethod]
        public void Serialization_Should_Be_Correct_For_Null_ComplexType_Property()
        {
            var instance = new SomeClassWithComplexTypesProperties();

            string actual = instance.AsXElement().ToString().Trim();

            string expected = @"
<SomeClassWithComplexTypesProperties>
  <Id>1</Id>
  <ComplexType />
</SomeClassWithComplexTypesProperties>".Trim();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Serialization_Should_Be_Correct_For_Non_Null_ComplexType_Property()
        {
            var instance = new SomeClassWithComplexTypesProperties
            {
                ComplexType = new SomeClassWithComplexTypesProperties.SomeClass()
            };

            string actual = instance.AsXElement().ToString().Trim();

            string expected = @"
<SomeClassWithComplexTypesProperties>
  <Id>1</Id>
  <ComplexType>ProdoctivityOpenXMLModel.Tests.ConvertibleToXElementTests.ConvertibleToXElement_Tests_For_ComplexTypes+SomeClassWithComplexTypesProperties+SomeClass</ComplexType>
</SomeClassWithComplexTypesProperties>".Trim();

            Assert.AreEqual(expected, actual);
        }
    }
}
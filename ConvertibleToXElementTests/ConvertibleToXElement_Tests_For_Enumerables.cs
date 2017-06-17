using AsXElement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;

namespace ConvertibleToXElementTests
{
    [TestClass]
    public class ConvertibleToXElement_Tests_For_Enumerables
    {
        public class SomeClass : ConvertibleToXElement
        {
            public int Id { get; set; } = 1;
        }

        public class SomeList : ConvertibleToXElement, ICollection<SomeClass>
        {
            private readonly ICollection<SomeClass> _items = new List<SomeClass>();

            public int Count => _items.Count;

            public bool IsReadOnly => _items.IsReadOnly;

            public void Add(SomeClass item) => _items.Add(item);
            public void Clear() => _items.Clear();
            public bool Contains(SomeClass item) => _items.Contains(item);
            public void CopyTo(SomeClass[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
            public IEnumerator<SomeClass> GetEnumerator() => _items.GetEnumerator();
            public bool Remove(SomeClass item) => _items.Remove(item);
            IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
        }

        [TestMethod]
        public void Serialization_Should_Be_Correct_For_ValueType_Enumerable()
        {
            var instance = new SomeList
            {
                new SomeClass(), new SomeClass()
            };

            string actual = instance.AsXElement().ToString().Trim();

            string expected = @"
<SomeList>
  <Count>2</Count>
  <IsReadOnly>false</IsReadOnly>
  <SomeClass>
    <Id>1</Id>
  </SomeClass>
  <SomeClass>
    <Id>1</Id>
  </SomeClass>
</SomeList>".Trim();

            Assert.AreEqual(expected, actual);
        }

    }
}
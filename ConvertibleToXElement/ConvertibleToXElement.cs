using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace AsXElement
{
    /// <summary>
    /// Exposes the <see cref="AsXElement"/> method that returns an <see cref="XElement"/>
    /// representations of all the current class' properties. If the class that inherits it
    /// is also an <see cref="IEnumerable"/>, it will include the items of the collections aswell.
    /// Also see <see cref="NonConvertibleToXElementAttribute"/>.
    /// </summary>
    public class ConvertibleToXElement
    {
        protected ConvertibleToXElement()
        {
        }

        /// <summary>
        /// Returns an <see cref="XElement"/> representations of all the current class' properties. 
        /// If the class current class is also an <see cref="IEnumerable"/>, it will include the items of the collections aswell.
        /// </summary>
        /// <returns></returns>
        public XElement AsXElement() => AsXElement(string.Empty);

        /// <summary>
        /// Returns an <see cref="XElement"/> representations of all the current class' properties. 
        /// If the class current class is also an <see cref="IEnumerable"/>, it will include the items of the collections aswell.
        /// </summary>
        /// <param name="xnamespace">Namespace to include in the <code>xlmns=""</code> attribute of the top node.</param>
        /// <returns></returns>
        public XElement AsXElement(XNamespace xnamespace)
        {
            if (xnamespace == null)
            {
                throw new ArgumentNullException(nameof(xnamespace));
            }

            // Get the current class properties as XElements
            IEnumerable<PropertyInfo> properties = GetType()
                                                    .GetTypeInfo().DeclaredProperties
                                                    .Where(p => !p.IsDefined(typeof(NonConvertibleToXElementAttribute)));
            IEnumerable<XElement> propertiesAsXElements = properties.Select(p =>
            {
                bool isEnumerableProperty = IsEnumerable(p.PropertyType);
                if (isEnumerableProperty)
                {
                    return GetXElementFromEnumerableProperty(p, xnamespace);
                }

                return GetXElementFromObject(p.Name, p.GetValue(this), xnamespace);
            });

            // If the current class is an enumerable,
            // get its items as XElements aswell
            var membersAsXElements = new List<XElement>();
            membersAsXElements.AddRange(propertiesAsXElements);
            if (IsEnumerable(GetType()))
            {
                foreach (object item in (IEnumerable<object>)this)
                {
                    XElement xelement = GetXElementFromObject(item?.GetType().Name, item, xnamespace);
                    membersAsXElements.Add(xelement);
                }
            }

            return new XElement(xnamespace + GetType().Name, membersAsXElements);
        }

        private static bool IsEnumerable(Type type) => type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IEnumerable)) && type != typeof(String);

        private XElement GetXElementFromEnumerableProperty(PropertyInfo p, XNamespace xnamespace) => GetXElementFromEnumerableProperty(p.Name, p.GetValue(this), xnamespace);

        private static XElement GetXElementFromEnumerableProperty(string propertyName, object propertyValue, XNamespace xnamespace)
        {
            IEnumerable values = propertyValue != null ? (IEnumerable)propertyValue : new List<object>();
            var xelement = new List<XElement>();
            foreach (object value in values)
            {
                XElement xelementItem = IsEnumerable(value.GetType()) ?
                    GetXElementFromEnumerableProperty(value.GetType().Name, value, xnamespace)
                    : GetXElementFromObject(value?.GetType().Name, value, xnamespace);
                xelement.Add(xelementItem);
            }
            return new XElement(xnamespace + propertyName, xelement);
        }

        private static XElement GetXElementFromObject(string propertyName, object value, XNamespace xnamespace)
        {
            if (value is ConvertibleToXElement)
            {
                return CastObjectAndConvertToXElement(value, xnamespace);
            }

            try
            {
                return GetXElementFromXmlString(value?.ToString(), xnamespace);
            }
            catch (Exception ex) when (ex is XmlException || ex is ArgumentNullException)
            {
                return new XElement(xnamespace + propertyName, value);
            }
        }

        private static XElement CastObjectAndConvertToXElement(object value, XNamespace xnamespace) => ((ConvertibleToXElement)value)?.AsXElement(xnamespace);

        private static XElement GetXElementFromXmlString(string str, XNamespace xnamespace)
        {
            XElement xelement = XElement.Parse(str);
            foreach (XElement descendants in xelement.DescendantsAndSelf())
            {
                descendants.Name = xnamespace + descendants.Name.LocalName;
            }
            return xelement;
        }
    }
}
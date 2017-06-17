using System;

namespace AsXElement
{
    /// <summary>
    /// Apply this attribute on class properties that should be ignored in the
    /// returned <see cref="XElement"/> of the <see cref="ConvertibleToXElement.AsXElement"/> method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class NonConvertibleToXElementAttribute : Attribute
    {
    }
}
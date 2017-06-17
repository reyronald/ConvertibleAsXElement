# ConvertibleAsXElement

Defines a class that can be inherited to expose the AsXElement() method, which returns an XElement representations of all the current class' properties. If the class that inherits it  is also an IEnumerable, it will include the items of the collections aswell.
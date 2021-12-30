using System;
using System.Linq;
using System.Collections.Generic;

namespace HelperAndToolsForUT.Helper.Test.FluentAssertions.Tools
{

    public interface IPunctuation
    {
        string AddExclamationPoint(string s);
    }

    public interface IInterface
    {
        void DoSomething(ComplexType complexType);
    }

    public class Punctuation : IPunctuation
    {
        public string AddExclamationPoint(string s)
        {
            return s + "!";
        }
    }

    public class ComplexType
    {
        public int IntProperty { get; set; }

        public string StringProperty { get; set; }

        public Guid? GuidProperty { get; set; }

        public AnotherComplexType ComplexTypeProperty { get; set; }

        public IEnumerable<string> ListProperty { get; set; }

        public class AnotherComplexType
        {
            public int IntProperty { get; set; }

            public string StringProperty { get; set; }
        }

        public ComplexType Copy()
        {
            return new ComplexType
            {
                IntProperty = IntProperty,
                StringProperty = StringProperty,
                GuidProperty = GuidProperty,
                ComplexTypeProperty = new AnotherComplexType
                {
                    IntProperty = ComplexTypeProperty.IntProperty,
                    StringProperty = ComplexTypeProperty.StringProperty
                },
                ListProperty = ListProperty.Select(s => s)
            };
        }
    }

}

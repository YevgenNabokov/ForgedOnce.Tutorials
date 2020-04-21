using System;
using System.Collections.Generic;
using System.Text;

namespace Source.Inputs
{
    public class TestClass
    {
        public int IntField;
        public int IntProperty
        {
            get;
            set;
        }

        public string StringField;
        public string StringProperty
        {
            get;
            set;
        }

        public System.String Print()
        {
            return $"IntProperty: '{IntProperty}'; StringProperty: '{StringProperty}'; IntField: '{IntField}'; StringField: '{StringField}';";
        }

        public override System.String ToString()
        {
            return Print();
        }
    }
}
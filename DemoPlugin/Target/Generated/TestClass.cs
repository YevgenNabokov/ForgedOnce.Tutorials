using System;
using System.Collections.Generic;
using System.Text;

namespace Source.Inputs
{
    public class TestClass : BaseTestClass
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

        public override System.String Print()
        {
            return $"IntProperty: '{IntProperty}'; IntField: '{IntField}'; BaseIntField: '{BaseIntField}';";
        }

        public override System.String ToString()
        {
            return Print();
        }
    }
}
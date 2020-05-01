using System;
using System.Collections.Generic;
using System.Text;

namespace Source.Inputs
{
    public class BaseTestClass
    {
        public int BaseIntField;
        public string BaseStringField;
        public virtual System.String Print()
        {
            return $"BaseIntField: '{BaseIntField}';";
        }

        public override System.String ToString()
        {
            return Print();
        }
    }
}
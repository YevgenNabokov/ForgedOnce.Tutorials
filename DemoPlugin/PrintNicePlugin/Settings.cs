using System;
using System.Collections.Generic;
using System.Text;

namespace PrintNicePlugin
{
    public class Settings
    {
        public const string OverrideToStringKey = "overrideToString";

        public const string PrintMethodNameKey = "printMethodName";

        public bool OverrideToString;

        public string PrintMethodName = "Print";
    }
}

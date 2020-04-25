using System;
using System.Collections.Generic;
using System.Text;

namespace PrintNicePlugin
{
    public class Settings
    {
        public const string OverrideToStringKey = "overrideToString";

        public const string PrintMethodNameKey = "printMethodName";

        public const string OnlyTypesToIncludeKey = "onlyTypesToInclude";

        public const string TypesToExcludeKey = "typesToExclude";

        public bool OverrideToString;

        public string PrintMethodName = "Print";

        public string[] OnlyTypesToInclude;

        public string[] TypesToExclude;
    }
}

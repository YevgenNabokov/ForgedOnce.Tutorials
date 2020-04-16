using System.Collections.Generic;

namespace PrintNicePlugin.Models
{
    public class ClassParameters
    {
        public string TypeName;

        public List<MemberParameters> Members = new List<MemberParameters>();

        public ClassParameters(string typeName)
        {
            this.TypeName = typeName;
        }
    }
}

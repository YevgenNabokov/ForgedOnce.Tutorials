namespace PrintNicePlugin.Models
{
    public class MemberParameters
    {
        public string Name;

        public bool Include;

        public MemberParameters(string name, bool include)
        {
            this.Name = name;
            this.Include = include;
        }
    }
}

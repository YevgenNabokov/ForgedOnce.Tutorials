using ForgedOnce.Launcher.MSBuild.Default;

namespace PluginLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            Launcher launcher = new Launcher();
            launcher.Launch(args[0], args[1]);
        }
    }
}

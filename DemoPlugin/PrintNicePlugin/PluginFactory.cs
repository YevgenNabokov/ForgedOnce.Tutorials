using ForgedOnce.Core.Interfaces;
using ForgedOnce.CSharp;
using ForgedOnce.Environment.Interfaces;
using Newtonsoft.Json.Linq;

namespace PrintNicePlugin
{
    public class PluginFactory : ICodeGenerationPluginFactory<Settings, Parameters, CodeFileCSharp>
    {
        public ICodeGenerationPlugin CreatePlugin(
            JObject configuration,
            IPluginPreprocessor<CodeFileCSharp, Parameters, Settings> pluginPreprocessor = null)
        {
            var settings = new Settings();
            return new Plugin()
            {
                Preprocessor = pluginPreprocessor ?? new Preprocessor(),
                Settings = settings
            };
        }
    }
}

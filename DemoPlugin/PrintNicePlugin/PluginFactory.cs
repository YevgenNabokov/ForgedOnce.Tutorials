using ForgedOnce.Core.Interfaces;
using ForgedOnce.CSharp;
using ForgedOnce.Environment.Interfaces;
using Newtonsoft.Json.Linq;
using System;

namespace PrintNicePlugin
{
    public class PluginFactory : ICodeGenerationPluginFactory<Settings, Parameters, CodeFileCSharp>
    {
        public ICodeGenerationPlugin CreatePlugin(
            JObject configuration,
            IPluginPreprocessor<CodeFileCSharp, Parameters, Settings> pluginPreprocessor = null)
        {
            var settings = new Settings();
            this.SetFromConfiguration((v) => bool.TryParse(v, out settings.OverrideToString), configuration, Settings.OverrideToStringKey);
            this.SetFromConfiguration((v) => settings.PrintMethodName = v, configuration, Settings.PrintMethodNameKey);

            return new Plugin()
            {
                Preprocessor = pluginPreprocessor ?? new Preprocessor(),
                Settings = settings
            };
        }

        private void SetFromConfiguration(Action<string> target, JObject configuration, string key)
        {
            if (configuration != null && configuration.ContainsKey(key))
            {
                target(configuration[key].Value<string>());
            }
        }
    }
}

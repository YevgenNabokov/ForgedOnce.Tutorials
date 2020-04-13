using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;

namespace PrintNicePlugin
{
    public class Preprocessor : IPluginPreprocessor<CodeFileCSharp, Parameters, Settings>
    {
        public Parameters GenerateMetadata(
            CodeFileCSharp input,
            Settings pluginSettings,
            IMetadataReader metadataReader,
            ILogger logger,
            IFileGroup<CodeFileCSharp, GroupItemDetails> fileGroup = null)
        {
            return new Parameters();
        }
    }
}

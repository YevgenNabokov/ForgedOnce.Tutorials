using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using System;
using System.Collections.Generic;

namespace PrintNicePlugin
{
    public class Plugin : CodeGenerationFromCSharpPlugin<Settings, Parameters>
    {
        public const string OutStreamName = "PassThrough";

        private ICodeStream passThroughStream;

        public Plugin()
        {
            this.Signature = new PluginSignature()
            {
                Id = "4DAB3B44-C7C1-4578-95EE-8252D873BB83",
                InputLanguage = Languages.CSharp,
                Name = typeof(Plugin).FullName
            };
        }

        protected override List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory)
        {
            List<ICodeStream> result = new List<ICodeStream>();
            this.passThroughStream = codeStreamFactory.CreateCodeStream(Languages.CSharp, OutStreamName);
            result.Add(this.passThroughStream);
            return result;
        }

        protected override void Implementation(CodeFileCSharp input, Parameters inputParameters, IMetadataRecorder metadataRecorder, ILogger logger)
        {
            throw new NotImplementedException();
        }
    }
}

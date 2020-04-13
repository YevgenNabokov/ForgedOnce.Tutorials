using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.CSharp;
using System;
using System.Collections.Generic;

namespace PrintNicePlugin
{
    public class Plugin : CodeGenerationFromCSharpPlugin<Settings, Parameters>
    {
        protected override List<ICodeStream> CreateOutputs(ICodeStreamFactory codeStreamFactory)
        {
            throw new NotImplementedException();
        }

        protected override void Implementation(CodeFileCSharp input, Parameters inputParameters, IMetadataRecorder metadataRecorder, ILogger logger)
        {
            throw new NotImplementedException();
        }
    }
}

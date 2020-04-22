using ForgedOnce.Core;
using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using ForgedOnce.CSharp.Helpers.SemanticAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var outFile = this.passThroughStream.CreateCodeFile(input.Name) as CodeFileCSharp;
            CSharpSyntaxUtils.CloneContent(input, outFile, metadataRecorder);

            var snapshot = outFile.NodePathService.GetSubTreeSnapshot(outFile.SyntaxTree.GetRoot());
            var editor = new SyntaxEditor(snapshot, input, outFile, inputParameters, this.Settings);
            var newRoot = editor.Visit(outFile.SyntaxTree.GetRoot());
            outFile.SyntaxTree = CSharpSyntaxTree.Create(newRoot as CSharpSyntaxNode);

            foreach (var added in outFile.SyntaxTree.GetRoot().DescendantNodes().Where((n) => n.GetAnnotations(editor.AnnotationKey).Any()))
            {
                metadataRecorder.SymbolGenerated<SyntaxNode>(outFile.NodePathService, added, new Dictionary<string, string>());
            }
        }
    }
}

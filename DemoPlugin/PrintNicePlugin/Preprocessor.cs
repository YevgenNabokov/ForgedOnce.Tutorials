using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using ForgedOnce.CSharp.Helpers.SemanticAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PrintNicePlugin.Models;
using System.Linq;

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
            var result = new Parameters();

            foreach (var classDeclaration in input.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var declaredSymbol = input.SemanticModel.GetDeclaredSymbol(classDeclaration);
                var typeName = declaredSymbol.GetFullMetadataName();

                ClassParameters classParams = new ClassParameters(typeName);

                foreach (var prop in declaredSymbol.GetAllSymbols<IPropertySymbol>(SymbolKind.Property, Accessibility.Public))
                {
                    classParams.Members.Add(new MemberParameters(prop.Name, true));
                }

                foreach (var field in declaredSymbol.GetAllSymbols<IFieldSymbol>(SymbolKind.Field, Accessibility.Public))
                {
                    classParams.Members.Add(new MemberParameters(field.Name, true));
                }
            }

            return result;
        }
    }
}

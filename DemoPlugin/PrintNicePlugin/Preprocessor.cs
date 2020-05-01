using ForgedOnce.Core.Interfaces;
using ForgedOnce.Core.Plugins;
using ForgedOnce.CSharp;
using ForgedOnce.CSharp.Helpers.SemanticAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PrintNicePlugin.Models;
using System;
using System.Linq;

namespace PrintNicePlugin
{
    public class Preprocessor : IPluginPreprocessor<CodeFileCSharp, Parameters, Settings>
    {
        public Parameters GenerateParameters(
            CodeFileCSharp input,
            Settings pluginSettings,
            IMetadataReader metadataReader,
            ILogger logger,
            IFileGroup<CodeFileCSharp, GroupItemDetails> fileGroup = null)
        {
            var onlyTypesToInclude = pluginSettings
                .OnlyTypesToInclude?
                .Select(t => input.SemanticModel.Compilation.GetTypeByMetadataName(t) 
                ?? throw new InvalidOperationException($"Unable to resolve '{t}' from {Settings.OnlyTypesToIncludeKey} list in plugin configuration.")).ToArray();
            var typesToExclude = pluginSettings
                .TypesToExclude?
                .Select(t => input.SemanticModel.Compilation.GetTypeByMetadataName(t)
                ?? throw new InvalidOperationException($"Unable to resolve '{t}' from {Settings.TypesToExcludeKey} list in plugin configuration.")).ToArray();

            var result = new Parameters();

            foreach (var classDeclaration in input.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                var declaredSymbol = input.SemanticModel.GetDeclaredSymbol(classDeclaration);
                var typeName = declaredSymbol.GetFullMetadataName();

                ClassParameters classParams = new ClassParameters(typeName);

                Func<INamedTypeSymbol, bool> typeIsDefinedInInputGroup = (baseType) =>
                    fileGroup
                            .Files
                            .Any(inputFile =>
                                inputFile.Key.SyntaxTree
                                .GetRoot()
                                .DescendantNodes()
                                .OfType<ClassDeclarationSyntax>()
                                .Any(inputClass => SymbolEqualityComparer.Default.Equals(inputFile.Key.SemanticModel.GetDeclaredSymbol(inputClass), baseType)));

                Func<INamedTypeSymbol, bool> typeContainsOverridableTargetMethod = (type) =>
                    type != null
                    && type.GetAllSymbols<IMethodSymbol>(SymbolKind.Method, Accessibility.Public)
                        .Any(m => m.Name == pluginSettings.PrintMethodName
                            && m.Parameters.Length == 0
                            && SymbolEqualityComparer.Default.Equals(m.ReturnType, input.SemanticModel.Compilation.GetTypeByMetadataName(typeof(string).ToString()))
                            && (m.IsVirtual || m.IsAbstract));

                classParams.Override =
                    (fileGroup != null
                    && declaredSymbol
                    .GetBaseTypes()
                    .Any(baseType => typeIsDefinedInInputGroup(baseType)))
                    || typeContainsOverridableTargetMethod(declaredSymbol.BaseType);                    

                foreach (var prop in declaredSymbol.GetAllSymbols<IPropertySymbol>(SymbolKind.Property, Accessibility.Public))
                {
                    classParams.Members.Add(new MemberParameters(prop.Name, this.Filter(onlyTypesToInclude, typesToExclude, prop.Type)));
                }

                foreach (var field in declaredSymbol.GetAllSymbols<IFieldSymbol>(SymbolKind.Field, Accessibility.Public))
                {
                    classParams.Members.Add(new MemberParameters(field.Name, this.Filter(onlyTypesToInclude, typesToExclude, field.Type)));
                }

                result.Classes.Add(classParams);
            }

            return result;
        }

        private bool Filter(INamedTypeSymbol[] onlyTypesToInclude, INamedTypeSymbol[] typesToExclude, ITypeSymbol type)
        {
            var result = true;
            if (onlyTypesToInclude != null)
            {
                result = onlyTypesToInclude.Any(t => type.InheritsFromOrImplementsOrEqualsIgnoringConstruction(t));
            }

            if (typesToExclude != null)
            {
                result = result && !typesToExclude.Any(t => type.InheritsFromOrImplementsOrEqualsIgnoringConstruction(t));
            }

            return result;
        }
    }
}

using ForgedOnce.Core.Metadata;
using ForgedOnce.Core.Metadata.Interfaces;
using ForgedOnce.CSharp;
using ForgedOnce.CSharp.Helpers.SemanticAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PrintNicePlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PrintNicePlugin
{
    public class SyntaxEditor : CSharpSyntaxRewriter
    {
        private readonly string printMethodName;
        private readonly ISubTreeSnapshot snapshot;
        private readonly CodeFileCSharp input;
        private readonly CodeFileCSharp output;
        private readonly Parameters inputParameters;

        public SyntaxEditor(ISubTreeSnapshot snapshot, CodeFileCSharp input, CodeFileCSharp output, Parameters inputParameters, string printMethodName)
        {
            this.snapshot = snapshot;
            this.input = input;
            this.output = output;
            this.inputParameters = inputParameters;
            this.printMethodName = printMethodName;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var originalNode = this.GetInputNode(node) as ClassDeclarationSyntax;
            var declaredSymbol = input.SemanticModel.GetDeclaredSymbol(originalNode);
            var typeName = declaredSymbol.GetFullMetadataName();

            var parameters = this.inputParameters.Classes.FirstOrDefault(p => p.TypeName == typeName);
            if (parameters != null)
            {
                if (declaredSymbol
                    .GetMembers(this.printMethodName)
                    .Any(m => (m.Kind == SymbolKind.Method && ((IMethodSymbol)m).Parameters.Length == 0) ||
                         m.Kind == SymbolKind.Property ||
                         m.Kind == SymbolKind.Field))
                {
                    throw new InvalidOperationException($"Target class {typeName} already contains method {this.printMethodName}.");
                }

                return this.AddPrint(node, parameters);
            }
            else
            {
                return node;
            }
        }

        private ClassDeclarationSyntax AddPrint(ClassDeclarationSyntax node, ClassParameters parameters)
        {
            var parts = new List<InterpolatedStringContentSyntax>();
            var includedMembers = parameters.Members.Where(m => m.Include).ToArray();
            for (var i = 0; i < includedMembers.Length; i++)
            {
                var stringStartPart = $"{ includedMembers[i].Name}: '";
                var stringEndPart = i == includedMembers.Length - 1 ? "';" : "'; ";
                parts.Add(InterpolatedStringText(Token(TriviaList(), SyntaxKind.InterpolatedStringTextToken, stringStartPart, stringStartPart, TriviaList())));
                parts.Add(Interpolation(IdentifierName(includedMembers[i].Name)));
                parts.Add(InterpolatedStringText(Token(TriviaList(), SyntaxKind.InterpolatedStringTextToken, stringEndPart, stringEndPart, TriviaList())));
            }

            var printMethod = MethodDeclaration(ParseTypeName(typeof(string).ToString()), this.printMethodName)
                .WithBody(
                Block(
                    List(new StatementSyntax[] { 
                        ReturnStatement(
                            InterpolatedStringExpression(
                                Token(SyntaxKind.InterpolatedStringStartToken),
                                List(parts)))
                    })))
                .WithModifiers(TokenList(new SyntaxToken[] { Token(SyntaxKind.PublicKeyword) }));

            return node.AddMembers(printMethod);
        }

        private SyntaxNode GetInputNode(SyntaxNode outputNode)
        {
            var originalPath = this.snapshot.GetNodeOriginalPath(this.output.NodePathService.GetNodePath(outputNode));
            var originalPathCorrected = this.input.NodePathService.ReplacePathRootWithThisFile(originalPath);
            return input.NodePathService.ResolveNode(originalPathCorrected);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using FiM_Compiler.CodeGeneration.Compilers.CSharp;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers
{
    public class CSharpCompiler : Compiler
    {
        private List<Token> _tokens;
        private SyntaxNode _syntaxTreeHead;

        
        public override void Compile(string sourceCode, string filename)
        {
            base.Compile(sourceCode, filename);
            InitialPreparing();
            ILexer lexer = new Lexer();
            _tokens = lexer.PerformLexicalAnalysis(this.SourceCode);

            //foreach (var cur in tokens)
            //    if (cur.Type != TokenType.Whitespace && cur.Type != TokenType.Newline)
            //        Console.WriteLine(cur.ToString());

            var parser = new Parser();
            _syntaxTreeHead = parser.GenerateSyntaxTree(_tokens, CompileErrors);
            if (_syntaxTreeHead == null)
            {
                return;
            }
            if (!parser.PerformTypesCheck(_syntaxTreeHead, CompileErrors))
            {
                return;
            }
            var output = parser.GenerateCode(_syntaxTreeHead);
            PrintResult(output);
        }


        private void InitialPreparing()
        {
            CompileErrors = new List<Error>();

            SourceCode = SourceCode.Replace("\r", "");
            if (SourceCode[SourceCode.Length - 1] != '\n')
                SourceCode += '\n';
        }

        private void PrintResult(string code)
        {
            OutputFilename = $@"{Filename.Replace(".", "_")}.cs";
            var outputPath = $@"{Environment.CurrentDirectory}\{Filename.Replace(".", "_")}.cs";
            using (var sourceWriter = new StreamWriter(outputPath))
            {
                sourceWriter.Write(code);
            }
        }


        #region Constructor

        #endregion
    }
}

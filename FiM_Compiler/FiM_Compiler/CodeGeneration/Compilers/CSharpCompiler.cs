using System;
using System.Collections.Generic;
using System.IO;

using FiM_Compiler.CodeGeneration.Compilers.CSharp;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers
{
    public class CSharpCompiler : Compiler
    {
        List<Token> tokens;
        SyntaxNode syntaxTreeHead;
        
        public override void Compile(string sourceCode, string filename)
        {
            base.Compile(sourceCode, filename);
            InitialPreparing();
            ILexer lexer = new Lexer();
            tokens = lexer.PerformLexicalAnalysis(this.sourceCode);

            //foreach (var cur in tokens)
            //    if (cur.Type != TokenType.Whitespace && cur.Type != TokenType.Newline)
            //        Console.WriteLine(cur.ToString());

            Parser parser = new Parser();
            syntaxTreeHead = parser.GenerateSyntaxTree(tokens, compileErrors);
            if (syntaxTreeHead != null)
            {
                if(parser.PerformTypesCheck(syntaxTreeHead, compileErrors))
                {
                    string output = parser.GenerateCode(syntaxTreeHead);
                    PrintResult(output);
                }
            }
        }

        void InitialPreparing()
        {
            compileErrors = new List<Error>() { };

            sourceCode = sourceCode.Replace("\r", "");
            if (sourceCode[sourceCode.Length - 1] != '\n')
                sourceCode += '\n';
        }

        bool IsWhitespace(char symbol)
        {
            return symbol == ' ' || symbol == '\t';
        }

        void PrintResult(string code)
        {
            outputFilename = string.Format(@"{0}.cs",
                     filename.Replace(".", "_"));

            string outputPath = string.Format(@"{0}\{1}.cs",
                     Environment.CurrentDirectory,
                     filename.Replace(".", "_"));
            using (StreamWriter sourceWriter = new StreamWriter(outputPath))
            {
                sourceWriter.Write(code);
            }
        }

        #region Constructor
        public CSharpCompiler() : base()
        {

        }
        #endregion
    }
}

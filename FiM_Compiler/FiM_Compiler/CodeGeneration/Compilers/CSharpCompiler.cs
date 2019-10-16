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
            ILexer lexer = new Lexer(this.sourceCode);
            bool status = true;
            (tokens, status) = lexer.PerformLexicalAnalysis(compileErrors);
            if(status)
            {
                foreach (var cur in tokens)
                    Console.WriteLine(cur.ToString());
            }
            //Parser parser = new Parser(tokens, compileErrors);
            //if (parser.GenerateSyntaxTree(ref syntaxTreeHead))
            //{
            //    string output = parser.GenerateCSharpCode(syntaxTreeHead);
            //    PrintResult(output);
            //}
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

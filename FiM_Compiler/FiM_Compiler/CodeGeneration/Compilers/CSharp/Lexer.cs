using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalysises;
using FiM_Compiler.CodeGeneration.Compilers.CSharp.ErrorsChecks;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp
{
    public class Lexer
    {
        private string sourceCode;

        private List<ILexerAnalysis> initialAnalyses;
        private List<ILexerErrorCheck> initialErrorChecks;
        private List<ILexerAnalysis> secondStepAnalyses;

        public (List<Token>, bool) PerformLexicalAnalysis(List<Error> compileErrors)
        {
            List<Token> tokens = new List<Token>();
            foreach(var cur in initialAnalyses)
            {
                tokens = cur.PerformLexicalAnalysis(tokens, sourceCode);
            }
            foreach(var cur in initialErrorChecks)
            {
                if (!cur.PerformChecks(tokens, compileErrors))
                    return (null, false);
            }
            foreach (var cur in secondStepAnalyses)
            {
                tokens = cur.PerformLexicalAnalysis(tokens, sourceCode);
            }
            return (tokens, true);
        }

        #region Constructor
        public Lexer(string sourceCode)
        {
            this.sourceCode = sourceCode;
            initialAnalyses = new List<ILexerAnalysis>()
            {
                new InitialAnalysis(), new KeywordDeclarationAnalysis()
            };
            initialErrorChecks = new List<ILexerErrorCheck>()
            {
                new AmountOfEntryPointsCheck(), new ChecksBounds(), new NamesErrorChecks()
                //TODO check names for literals and keywords containing
            };
            secondStepAnalyses = new List<ILexerAnalysis>()
            {
                //TODO methods calling
            };
        }
        #endregion
    }
}

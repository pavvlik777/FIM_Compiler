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
    public class Lexer : ILexer
    {
        private string sourceCode;

        private List<ILexerAnalysis> initialAnalyses;
        private List<ILexerAnalysis> keywordAnalyses;
        private List<ILexerAnalysis> secondStepAnalyses;
        private List<ILexerAnalysis> thirdStepAnalyses;

        private List<ILexerErrorCheck> initialErrorChecks;
        private List<ILexerErrorCheck> keywordErrorChecks;
        private List<ILexerErrorCheck> secondStepErrorChecks;

        public (List<Token>, bool) PerformLexicalAnalysis(List<Error> compileErrors)
        {
            List<Token> tokens = new List<Token>();
            foreach(var cur in initialAnalyses)
            {
                tokens = cur.PerformLexicalAnalysis(tokens, sourceCode);
            }
            foreach (var cur in initialErrorChecks)
            {
                if (!cur.PerformChecks(tokens, compileErrors))
                    return (null, false);
            }

            foreach (var cur in keywordAnalyses)
            {
                tokens = cur.PerformLexicalAnalysis(tokens, sourceCode);
            }
            foreach (var cur in keywordErrorChecks)
            {
                if (!cur.PerformChecks(tokens, compileErrors))
                    return (null, false);
            }

            foreach (var cur in secondStepAnalyses)
            {
                tokens = cur.PerformLexicalAnalysis(tokens, sourceCode);
            }
            foreach (var cur in secondStepErrorChecks)
            {
                if (!cur.PerformChecks(tokens, compileErrors))
                    return (null, false);
            }

            foreach (var cur in thirdStepAnalyses)
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
                new InitialAnalysis(), new CommentsAnalysis(), new WhitespacesWithNames()
            };
            keywordAnalyses = new List<ILexerAnalysis>()
            {
                new KeywordDeclarationAnalysis()
            };
            secondStepAnalyses = new List<ILexerAnalysis>()
            {
                new MethodsCallingAnalysis(),
                new ArifmeticAnalysis(),
                new UserInteractionAnalysis(),
                new StatementsAndLoopsAnalysis(),
            };
            thirdStepAnalyses = new List<ILexerAnalysis>()
            {
                new VariableModifiersAnalysis(),
            };

            initialErrorChecks = new List<ILexerErrorCheck>()
            {
                new CommentsChecks()
            };
            keywordErrorChecks = new List<ILexerErrorCheck>()
            {
                new AmountOfEntryPointsCheck(), new ChecksBounds(), new NamesErrorChecks(),
                //TODO check names for containing literals and keywords
            };
            secondStepErrorChecks = new List<ILexerErrorCheck>()
            {
                new ExtendedNamesErrorChecks(),
            };
        }
        #endregion
    }
}

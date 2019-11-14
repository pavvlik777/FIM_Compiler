using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalysises;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp
{
    public class Lexer : ILexer
    {
        private List<ILexerAnalysis> lexerAnalyses;

        public List<Token> PerformLexicalAnalysis(string sourceCode)
        {
            List<Token> tokens = new List<Token>();
            foreach(var cur in lexerAnalyses)
            {
                tokens = cur.PerformLexicalAnalysis(tokens, sourceCode);
            }
            return tokens;
        }

        #region Constructor
        public Lexer()
        {
            lexerAnalyses = new List<ILexerAnalysis>()
            {
                new InitialAnalysis(), 
                new CommentsAnalysis(),
                new LiteralAnalysis(),
                new WhitespacesWithNames(),
                new KeywordDeclarationAnalysis(),
                new MethodNamesAnalysis(),
                new VariablesNamesAnalysis(),
                new MethodsCallingAnalysis(),
                new ArifmeticAnalysis(),
                new UserInteractionAnalysis(),
                new StatementsAndLoopsAnalysis(),
                new VariableModifiersAnalysis(),
            };
        }
        #endregion
    }
}

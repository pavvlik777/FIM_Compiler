using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalyses;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp
{
    public class Lexer : ILexer
    {
        private readonly List<ILexerAnalysis> _lexerAnalyses;


        public List<Token> PerformLexicalAnalysis(string sourceCode)
        {
            var tokens = new List<Token>();
            foreach (var cur in _lexerAnalyses)
            {
                tokens = cur.PerformLexicalAnalysis(tokens, sourceCode);
            }

            return tokens;
        }


        #region Constructor
        public Lexer()
        {
            _lexerAnalyses = new List<ILexerAnalysis>()
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

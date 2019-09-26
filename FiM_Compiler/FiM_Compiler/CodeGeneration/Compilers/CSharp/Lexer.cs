using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalysises;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp
{
    public class Lexer
    {
        private string sourceCode;

        private List<ILexerAnalysis> analyses;

        public List<Token> PerformLexicalAnalysis()
        {
            List<Token> tokens = new List<Token>();
            foreach(var cur in analyses)
            {
                tokens = cur.PerformLexicalAnalysis(tokens, sourceCode);
            }
            return tokens;
        }

        #region Constructor
        public Lexer(string sourceCode)
        {
            this.sourceCode = sourceCode;
            analyses = new List<ILexerAnalysis>()
            {
                new InitialAnalysis(), new KeywordAnalysis()
            };
        }
        #endregion
    }
}

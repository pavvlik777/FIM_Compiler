using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.InitialTokenRules;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalyses
{
    public class WhitespacesWithNames : ILexerAnalysis
    {
        List<TokenRule> mergeNamesRules;
        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            var initStack = new List<Token>(tokens);
            var stack = new List<Token>();
            var i = 0;
            while (i < initStack.Count)
            {
                if (!CheckStackForPatterns(stack, mergeNamesRules))
                {
                    stack.Add(initStack[i]);
                    i++;
                }
            }
            return stack;
        }

        bool CheckStackForPatterns(List<Token> tokens, List<TokenRule> rules)
        {
            var output = false;
            foreach (var cur in rules)
                if (cur.IsStackMatch(tokens))
                {
                    output = true;
                    break;
                }
            return output;
        }

        #region Constructor
        public WhitespacesWithNames()
        {
            mergeNamesRules = new List<TokenRule>()
            {
                new NameWithWhitespaceMergeRule()
            };
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.UserInteractions;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalysises
{
    public class UserInteractionAnalysis : ILexerAnalysis
    {
        private List<TokenRule> rules;

        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            List<Token> initStack = new List<Token>(tokens);
            List<Token> stack = new List<Token>();
            int i = 0;
            while (i < initStack.Count)
            {
                if (!CheckStackForPatterns(stack, rules))
                {
                    stack.Add(initStack[i]);
                    i++;
                }
            }
            return stack;
        }

        bool CheckStackForPatterns(List<Token> tokens, List<TokenRule> rules)
        {
            bool output = false;
            foreach (var cur in rules)
                if (cur.IsStackMatch(tokens))
                {
                    output = true;
                    break;
                }
            return output;
        }

        #region Constructor
        public UserInteractionAnalysis()
        {
            rules = new List<TokenRule>()
            {
                new Output(), new Input(), new Prompt()
            };
            Sort(rules);
        }

        void Sort(List<TokenRule> rules)
        {
            for (int i = 0; i < rules.Count - 1; i++) // Comment this if need specific order
            {
                for (int j = i + 1; j < rules.Count; j++)
                {
                    if (rules[i].Amount < rules[j].Amount)
                    {
                        TokenRule temp = rules[i];
                        rules[i] = rules[j];
                        rules[j] = temp;
                    }
                }
            }
        }
        #endregion
    }
}

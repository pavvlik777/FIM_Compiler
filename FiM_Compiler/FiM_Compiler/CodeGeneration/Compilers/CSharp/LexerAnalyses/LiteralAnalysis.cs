using System;
using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Literal;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalyses
{
    public class LiteralAnalysis : ILexerAnalysis
    {
        private List<TokenRule> rules;

        private enum Status
        { Nothing, Literal }
        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            var initStack = new List<Token>(tokens);
            var stack = new List<Token>();
            var i = 0;
            while (i < initStack.Count)
            {
                if (!CheckStackForPatterns(stack, rules))
                {
                    stack.Add(initStack[i]);
                    i++;
                }
            }
            initStack = new List<Token>(stack);
            stack = new List<Token>();
            i = 0;
            var start = 0;
            var status = Status.Nothing;
            while (i < initStack.Count)
            {
                stack.Add(initStack[i]);
                switch (status)
                {
                    case Status.Nothing:
                        if (initStack[i].Type == TokenType.Delimeters)
                        {
                            status = Status.Literal;
                            start = i;
                        }
                        break;
                    case Status.Literal:
                        if (initStack[i].Type == TokenType.Delimeters)
                        {
                            status = Status.Nothing;
                            ConvertTokens(ref stack, i - start + 1, TokenType.StringLiteral);
                        }
                        break;
                    default: throw new NotImplementedException($"Incorrect status {status.ToString()}");
                }
                i++;
            }
            return stack;
        }

        void ConvertTokens(ref List<Token> stack, int amount, TokenType newType, List<Token> childs = null)
        {
            var value = "";
            Token newToken = null;
            for (var j = amount; j >= 1; j--)
                value += stack[stack.Count - j].Value;
            if (childs != null)
                newToken = new Token(newType, value, new List<Token>(childs));
            else
                newToken = new Token(newType, value);
            for (var j = 1; j <= amount; j++)
                stack.RemoveAt(stack.Count - 1);
            stack.Add(newToken);
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
        public LiteralAnalysis()
        {
            rules = new List<TokenRule>()
            {
                new BoolLiteral(), new NullLiteral(),
                new CharLiteral(), new IntLiteral(), 
            };
            Sort(rules);
        }

        void Sort(List<TokenRule> rules)
        {
            for (var i = 0; i < rules.Count - 1; i++) // Comment this if need specific order
            {
                for (var j = i + 1; j < rules.Count; j++)
                {
                    if (rules[i].Amount < rules[j].Amount)
                    {
                        var temp = rules[i];
                        rules[i] = rules[j];
                        rules[j] = temp;
                    }
                }
            }
        }
        #endregion
    }
}

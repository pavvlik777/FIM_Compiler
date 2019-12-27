using System;
using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Comments;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalyses
{
    public class CommentsAnalysis : ILexerAnalysis
    {
        List<TokenRule> initialRules;
        private enum Status
        { Nothing, Inline, Multiline }

        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            var initStack = new List<Token>(tokens);
            var stack = new List<Token>();
            var i = 0;
            while (i < initStack.Count)
            {
                if (!CheckStackForPatterns(ref stack, initialRules))
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
            while(i < initStack.Count)
            {
                stack.Add(initStack[i]);
                switch (status)
                {
                    case Status.Nothing:
                        if (initStack[i].Type == TokenType.InlineCommentStart)
                        {
                            status = Status.Inline;
                            start = i;
                        }
                        else if (initStack[i].Type == TokenType.OpenMultilineComments)
                        {
                            status = Status.Multiline;
                            start = i;
                        }
                        break;
                    case Status.Inline:
                        if(initStack[i].Type == TokenType.Newline)
                        {
                            status = Status.Nothing;
                            ConvertTokens(ref stack, i - start + 1, TokenType.InlineComment);
                        }
                        break;
                    case Status.Multiline:
                        if (initStack[i].Type == TokenType.CloseMultilineComments)
                        {
                            status = Status.Nothing;
                            ConvertTokens(ref stack, i - start + 1, TokenType.MultilineComment);
                        }
                        break;
                    default: throw new NotImplementedException($"Incorrect status {status.ToString()}");
                }
                i++;
            }
            return stack;
        }

        bool CheckStackForPatterns(ref List<Token> tokens, List<TokenRule> rules)
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

        #region Constructor
        public CommentsAnalysis()
        {
            initialRules = new List<TokenRule>()
            {
                new InlineCommentStartRule(), new InlineCommentMergeRule()
            };
            Sort(ref initialRules);
        }

        void Sort(ref List<TokenRule> rules)
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

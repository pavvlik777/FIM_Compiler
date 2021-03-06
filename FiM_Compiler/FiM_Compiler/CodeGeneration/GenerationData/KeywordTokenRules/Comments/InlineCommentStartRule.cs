﻿using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Comments
{
    public class InlineCommentStartRule : TokenRule
    {
        public InlineCommentStartRule()
        {
            returnType = TokenType.InlineCommentStart;
            rule = new TokenType[] {
                TokenType.Name, TokenType.Punctuation, TokenType.Name, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 1].Value == "." && stack[stack.Count - 2].Value == "S" && stack[stack.Count - 3].Value == "." && stack[stack.Count - 4].Value == "P")
                {
                    PerformRuleTransform(stack);
                    return true;
                }
            }
            return false;
        }

        protected override void PerformRuleTransform(List<Token> stack)
        {
            ConvertTokens(ref stack, rule.Length, returnType);
        }
    }
}

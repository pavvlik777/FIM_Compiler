﻿using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.StatementsAndLoops
{
    public class CycleEnding : TokenRule
    {
        public CycleEnding()
        {
            returnType = TokenType.CycleEnding;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.CycleEnding, stack[stack.Count - 2].Value)
                    && stack[stack.Count - 1].Value == ".")
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

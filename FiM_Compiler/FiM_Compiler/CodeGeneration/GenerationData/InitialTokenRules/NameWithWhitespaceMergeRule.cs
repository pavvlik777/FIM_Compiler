﻿using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.InitialTokenRules
{
    public class NameWithWhitespaceMergeRule : TokenRule
    {
        public NameWithWhitespaceMergeRule()
        {
            returnType = TokenType.Name;
            rule = new TokenType[]
            {
                TokenType.Name, TokenType.Whitespace, TokenType.Name
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[]
                {
                    TokenType.Name, TokenType.SingleSpace, TokenType.Name
                }
            };
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                PerformRuleTransform(stack);
                return true;
            }
            foreach (var curRule in variations)
            {
                if (DefaultStackCheck(stack, curRule))
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

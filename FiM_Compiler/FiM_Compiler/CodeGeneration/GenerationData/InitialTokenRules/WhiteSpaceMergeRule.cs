using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.InitialTokenRules
{
    public class WhiteSpaceMergeRule : TokenRule
    {
        public WhiteSpaceMergeRule()
        {
            returnType = TokenType.Whitespace;
            rule = new TokenType[]
            {
                TokenType.Whitespace, TokenType.Whitespace
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[]
                {
                    TokenType.Whitespace, TokenType.SingleSpace
                },
                new TokenType[]
                {
                    TokenType.SingleSpace, TokenType.Whitespace
                },
                new TokenType[]
                {
                    TokenType.SingleSpace, TokenType.SingleSpace
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

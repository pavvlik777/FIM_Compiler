using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class MethodReturn : TokenRule
    {
        public MethodReturn()
        {
            returnType = TokenType.MethodReturn;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.Name, TokenType.Punctuation
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[]
                {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.BoolLiteral, TokenType.Punctuation
                },
                new TokenType[]
                {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.CharLiteral, TokenType.Punctuation
                },
                new TokenType[]
                {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.IntLiteral, TokenType.Punctuation
                },
                new TokenType[]
                {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.NullLiteral, TokenType.Punctuation
                },
                new TokenType[]
                {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.StringLiteral, TokenType.Punctuation
                }
            };
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 4].Value == "Then you get")
                {
                    PerformRuleTransform(ref stack);
                    return true;
                }
            }
            return false;
        }

        protected override void PerformRuleTransform(ref List<Token> stack)
        {
            List<Token> childsInput = new List<Token>();
            childsInput.Add(stack[stack.Count - 2]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

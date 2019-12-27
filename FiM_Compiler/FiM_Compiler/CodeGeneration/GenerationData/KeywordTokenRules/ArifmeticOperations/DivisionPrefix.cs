using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ArifmeticOperations
{
    public class DivisionPrefix : TokenRule
    {
        public DivisionPrefix()
        {
            returnType = TokenType.ArifmeticDivision;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.IntValue, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.IntValue
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.ArifmeticDivisionPrefixFirst, stack[stack.Count - 7].Value)
                    && KeywordsDictionary.IsKeyword(KeywordType.ArifmeticDivisionPrefixSecond, stack[stack.Count - 3].Value))
                {
                    PerformRuleTransform(stack);
                    return true;
                }
            }
            return false;
        }

        protected override void PerformRuleTransform(List<Token> stack)
        {
            var childsInput = new List<Token>();
            childsInput.Add(stack[stack.Count - 5]);
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

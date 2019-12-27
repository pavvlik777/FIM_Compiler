using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.BooleanOperations
{
    public class IsLessThanOrEqual : TokenRule
    {
        public IsLessThanOrEqual()
        {
            returnType = TokenType.LessThanOrEqual;
            rule = new TokenType[] {
                TokenType.Value, TokenType.Whitespace, TokenType.Keyword, TokenType.SingleSpace, TokenType.Keyword, TokenType.Whitespace, TokenType.Value
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.BooleanComparatorNot, stack[stack.Count - 5].Value)
                    && KeywordsDictionary.IsKeyword(KeywordType.BooleanGreaterThan, stack[stack.Count - 3].Value))
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
            childsInput.Add(stack[stack.Count - 7]);
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

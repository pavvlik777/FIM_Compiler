using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.BooleanOperators
{
    public class BooleanNot : TokenRule
    {
        public BooleanNot()
        {
            returnType = TokenType.BooleanNot;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.BoolValue
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.BooleanNot, stack[stack.Count - 3].Value))
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
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

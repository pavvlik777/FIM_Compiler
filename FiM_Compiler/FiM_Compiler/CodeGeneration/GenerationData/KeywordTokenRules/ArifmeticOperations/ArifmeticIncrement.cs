using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ArifmeticOperations
{
    public class ArifmeticIncrement : TokenRule
    {
        public ArifmeticIncrement()
        {
            returnType = TokenType.ArifmeticIncrement;
            rule = new TokenType[] {
                TokenType.IntValue, TokenType.Whitespace, TokenType.Keyword
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.ArifmeticIncrement, stack[stack.Count - 1].Value))
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
            childsInput.Add(stack[stack.Count - 3]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

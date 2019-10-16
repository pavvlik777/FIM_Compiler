using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.InitialTokenRules
{
    public class CharToNameRule : TokenRule
    {
        public CharToNameRule()
        {
            returnType = TokenType.Name;
            rule = new TokenType[]
            {
                TokenType.Char
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                PerformRuleTransform(stack);
                return true;
            }
            return false;
        }

        protected override void PerformRuleTransform(List<Token> stack)
        {
            ConvertTokens(ref stack, rule.Length, returnType);
        }
    }
}

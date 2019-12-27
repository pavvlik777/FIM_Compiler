using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Literal
{
    public class IntLiteral : TokenRule
    {
        public IntLiteral()
        {
            returnType = TokenType.IntLiteral;
            rule = new TokenType[] {
                TokenType.Name
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (int.TryParse(stack[stack.Count - 1].Value, out _))
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

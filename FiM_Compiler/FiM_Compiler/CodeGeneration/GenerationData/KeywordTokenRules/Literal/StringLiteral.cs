using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Literal
{
    public class StringLiteral : TokenRule
    {
        public StringLiteral()
        {
            returnType = TokenType.StringLiteral;
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
                if (stack[stack.Count - 1].Value.Length >= 2)
                {
                    if (stack[stack.Count - 1].Value[0] == '"' &&  stack[stack.Count - 1].Value[stack[stack.Count - 1].Value.Length - 1] == '"')
                    {
                        PerformRuleTransform(stack);
                        return true;
                    }
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

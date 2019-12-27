using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Literal
{
    public class BoolLiteral : TokenRule
    {
        public BoolLiteral()
        {
            returnType = TokenType.BoolLiteral;
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
                if (stack[stack.Count - 1].Value == "yes" || stack[stack.Count - 1].Value == "true" || stack[stack.Count - 1].Value == "right" || stack[stack.Count - 1].Value == "correct" ||
                    stack[stack.Count - 1].Value == "no" || stack[stack.Count - 1].Value == "false" || stack[stack.Count - 1].Value == "wrong" || stack[stack.Count - 1].Value == "incorrect")
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

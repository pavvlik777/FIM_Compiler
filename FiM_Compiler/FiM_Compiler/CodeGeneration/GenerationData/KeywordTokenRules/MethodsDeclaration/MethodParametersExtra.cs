using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class MethodParametersExtra : TokenRule
    {
        public MethodParametersExtra()
        {
            returnType = TokenType.MethodParametersExtra;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.VariableType, TokenType.Whitespace, TokenType.Name
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 5].Value == "and")
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
            childsInput.Add(stack[stack.Count - 3]);
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

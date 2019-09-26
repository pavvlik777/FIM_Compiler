using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class MethodParametersMerge : TokenRule
    {
        public MethodParametersMerge()
        {
            returnType = TokenType.MethodParameters;
            rule = new TokenType[] {
                TokenType.MethodParameters, TokenType.Whitespace, TokenType.MethodParametersExtra
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                PerformRuleTransform(ref stack);
                return true;
            }
            return false;
        }

        protected override void PerformRuleTransform(ref List<Token> stack)
        {
            List<string> childsInput = new List<string>();
            childsInput.AddRange(stack[stack.Count - 3].Childs);
            childsInput.AddRange(stack[stack.Count - 1].Childs);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

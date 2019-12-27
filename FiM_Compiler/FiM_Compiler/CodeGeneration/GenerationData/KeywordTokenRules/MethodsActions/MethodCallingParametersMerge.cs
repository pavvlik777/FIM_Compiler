using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.MethodsActions
{
    public class MethodCallingParametersMerge : TokenRule
    {
        public MethodCallingParametersMerge()
        {
            returnType = TokenType.MethodCallingParameters;
            rule = new TokenType[] {
                TokenType.MethodCallingParameters, TokenType.Whitespace, TokenType.MethodCallingParametersExtra
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
            var childsInput = new List<Token>();
            childsInput.AddRange(stack[stack.Count - 3].Childs);
            childsInput.AddRange(stack[stack.Count - 1].Childs);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

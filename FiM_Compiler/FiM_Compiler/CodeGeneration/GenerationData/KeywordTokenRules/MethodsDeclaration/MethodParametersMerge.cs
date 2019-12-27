using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.MethodsDeclaration
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

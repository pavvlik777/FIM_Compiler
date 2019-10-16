using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class MethodCallingWithParameters : TokenRule
    {
        public MethodCallingWithParameters()
        {
            returnType = TokenType.MethodCalling;
            rule = new TokenType[] {
                TokenType.MethodCalling, TokenType.Whitespace, TokenType.MethodCallingParameters
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[]
                {
                TokenType.MethodCalling, TokenType.Whitespace, TokenType.MethodCallingParametersExtra
                }
            };
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                PerformRuleTransform(stack);
                return true;
            }
            foreach (var curRule in variations)
            {
                if (DefaultStackCheck(stack, curRule))
                {
                    PerformRuleTransform(stack);
                    return true;
                }
            }
            return false;
        }

        protected override void PerformRuleTransform(List<Token> stack)
        {
            List<Token> childsInput = new List<Token>();
            childsInput.Add(stack[stack.Count - 3]);
            childsInput.AddRange(stack[stack.Count - 1].Childs);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

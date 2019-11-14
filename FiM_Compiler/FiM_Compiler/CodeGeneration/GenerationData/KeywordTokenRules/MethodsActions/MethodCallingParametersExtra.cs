using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class MethodCallingParametersExtra : TokenRule
    {
        public MethodCallingParametersExtra()
        {
            returnType = TokenType.MethodCallingParametersExtra;
            rule = new TokenType[] {
                TokenType.MethodCallingParameters, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.Value
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[]
                {
                    TokenType.MethodCalling, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.Value
                }
            };
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.MethodCallingParametersExtra, stack[stack.Count - 3].Value))
                {
                    PerformRuleTransform(stack);
                    return true;
                }
            }
            foreach (var curRule in variations)
            {
                if (DefaultStackCheck(stack, curRule))
                {
                    if (KeywordsDictionary.IsKeyword(KeywordType.MethodCallingParametersExtra, stack[stack.Count - 3].Value))
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
            List<Token> childsInput = new List<Token>();
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, 3, returnType, childsInput);
        }
    }
}

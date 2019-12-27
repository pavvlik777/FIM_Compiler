using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.VariableModifiers
{
    public class VariableRewriting : TokenRule
    {
        public VariableRewriting()
        {
            returnType = TokenType.VariableRewriting;
            rule = new TokenType[] {
                TokenType.VariableName, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.Value, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.VariableRewriting, stack[stack.Count - 4].Value))
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
            childsInput.Add(stack[stack.Count - 6]);
            childsInput.Add(stack[stack.Count - 2]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

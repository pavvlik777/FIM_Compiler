using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Variables
{
    public class VariableDeclarationWithType : TokenRule
    {
        public VariableDeclarationWithType()
        {
            returnType = TokenType.VariableDeclaration;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.VariableType, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.VariableDeclaration, stack[stack.Count - 8].Value) &&
                    KeywordsDictionary.IsKeyword(KeywordType.VariableDeclarationSecond, stack[stack.Count - 4].Value) &&
                    stack[stack.Count - 1].Value == "?")
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

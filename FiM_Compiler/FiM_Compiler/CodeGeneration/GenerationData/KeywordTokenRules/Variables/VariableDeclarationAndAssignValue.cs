using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class VariableDeclarationAndAssignValue : TokenRule
    {
        public VariableDeclarationAndAssignValue()
        {
            returnType = TokenType.VariableDeclarationAndAssign;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.VariableType, TokenType.Whitespace, TokenType.Name, TokenType.Punctuation
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[]
                {
                    TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.VariableType, TokenType.Whitespace, TokenType.MethodCalling, TokenType.Punctuation
                },
                new TokenType[]
                {
                    TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.VariableType, TokenType.Whitespace, TokenType.Literal, TokenType.Punctuation
                }
            };
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.VariableDeclaration, stack[stack.Count - 10].Value) &&
                    KeywordsDictionary.IsKeyword(KeywordType.VariableDeclarationSecond, stack[stack.Count - 6].Value) &&
                    stack[stack.Count - 1].Value == "?")
                {
                    PerformRuleTransform(stack);
                    return true;
                }
            }
            foreach (var curRule in variations)
            {
                if (DefaultStackCheck(stack, curRule))
                {
                    if (KeywordsDictionary.IsKeyword(KeywordType.VariableDeclaration, stack[stack.Count - 10].Value) &&
                        KeywordsDictionary.IsKeyword(KeywordType.VariableDeclarationSecond, stack[stack.Count - 6].Value) &&
                        stack[stack.Count - 1].Value == "?")
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
            childsInput.Add(stack[stack.Count - 8]);
            childsInput.Add(stack[stack.Count - 4]);
            childsInput.Add(stack[stack.Count - 2]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

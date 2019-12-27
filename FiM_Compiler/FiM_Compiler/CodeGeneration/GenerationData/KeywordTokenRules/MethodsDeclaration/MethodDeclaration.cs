using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.MethodsDeclaration
{
    public class MethodDeclaration : TokenRule
    {
        public MethodDeclaration()
        {
            returnType = TokenType.MethodDeclaration;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.MethodDeclaration, stack[stack.Count - 4].Value))
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
            childsInput.Add(stack[stack.Count - 2]);
            childsInput.Add(new Token(TokenType.MethodReturn, "void"));
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

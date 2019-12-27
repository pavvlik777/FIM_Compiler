using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.MethodsDeclaration
{
    public class MethodWithParametersDeclaration : TokenRule
    {
        public MethodWithParametersDeclaration()
        {
            returnType = TokenType.MethodDeclaration;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Whitespace, TokenType.MethodParameters, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.MethodDeclaration, stack[stack.Count - 6].Value))
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
            childsInput.Add(stack[stack.Count - 4]);
            childsInput.Add(new Token(TokenType.MethodReturn, "void"));
            childsInput.AddRange(stack[stack.Count - 2].Childs);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
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

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 6].Value == "I learned")
                {
                    PerformRuleTransform(ref stack);
                    return true;
                }
            }
            return false;
        }

        protected override void PerformRuleTransform(ref List<Token> stack)
        {
            List<Token> childsInput = new List<Token>();
            childsInput.Add(stack[stack.Count - 4]);
            childsInput.Add(new Token(TokenType.MethodReturn, "void"));
            childsInput.AddRange(stack[stack.Count - 2].Childs);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

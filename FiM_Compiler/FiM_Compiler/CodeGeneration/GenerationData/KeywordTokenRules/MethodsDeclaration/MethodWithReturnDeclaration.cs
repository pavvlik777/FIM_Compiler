using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class MethodWithReturnDeclaration : TokenRule
    {
        public MethodWithReturnDeclaration()
        {
            returnType = TokenType.MethodDeclaration;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Whitespace, TokenType.Keyword, TokenType.SingleSpace, TokenType.VariableType, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 8].Value == "I learned" && (stack[stack.Count - 4].Value == "with" || stack[stack.Count - 4].Value == "to get"))
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
            childsInput.Add(stack[stack.Count - 6]);
            childsInput.Add(stack[stack.Count - 2]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

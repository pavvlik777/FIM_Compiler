using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class EndMethodDeclaration : TokenRule
    {
        public EndMethodDeclaration()
        {
            returnType = TokenType.MethodEndDeclaration;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 4].Value == "That's all about")
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
            childsInput.Add(stack[stack.Count - 2]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

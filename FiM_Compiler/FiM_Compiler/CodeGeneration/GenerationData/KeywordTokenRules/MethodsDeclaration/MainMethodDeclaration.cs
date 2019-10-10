using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class MainMethodDeclaration : TokenRule
    {
        public MainMethodDeclaration()
        {
            returnType = TokenType.MainMethodDeclaration;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.MethodDeclaration
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 3].Value == "Today")
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
            foreach (var cur in stack[stack.Count - 1].Childs)
                childsInput.Add(cur);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

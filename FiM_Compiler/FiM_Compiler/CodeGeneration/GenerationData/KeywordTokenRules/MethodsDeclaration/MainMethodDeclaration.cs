using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.MethodsDeclaration
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

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.MainMethodDeclaration, stack[stack.Count - 3].Value))
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
            foreach (var cur in stack[stack.Count - 1].Childs)
                childsInput.Add(cur);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

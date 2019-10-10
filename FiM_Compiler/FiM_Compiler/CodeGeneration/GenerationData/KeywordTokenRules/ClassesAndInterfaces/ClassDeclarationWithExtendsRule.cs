using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class ClassDeclarationWithExtendsRule : TokenRule
    {
        public ClassDeclarationWithExtendsRule()
        {
            returnType = TokenType.ClassDeclaration;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.Name, TokenType.Whitespace, TokenType.ClassExtends, TokenType.Punctuation, TokenType.Whitespace, TokenType.Name, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 9].Value == "Dear")
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
            childsInput.Add(stack[stack.Count - 7]);
            childsInput.Add(stack[stack.Count - 2]);
            foreach (var cur in stack[stack.Count - 5].Childs)
                childsInput.Add(cur);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class InterfaceDeclarationWithExtends : TokenRule
    {
        public InterfaceDeclarationWithExtends()
        {
            returnType = TokenType.InterfaceDeclaration;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.Name, TokenType.Whitespace, TokenType.ClassExtends, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 6].Value == "Dearest")
                {
                    PerformRuleTransform(ref stack);
                    return true;
                }
            }
            return false;
        }

        protected override void PerformRuleTransform(ref List<Token> stack)
        {
            List<string> childsInput = new List<string>();
            childsInput.Add(stack[stack.Count - 4].Value);
            foreach (var cur in stack[stack.Count - 2].Childs)
                childsInput.Add(cur);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

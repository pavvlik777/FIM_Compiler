using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class InterfaceDeclarationWithExtends : TokenRule
    {
        public InterfaceDeclarationWithExtends()
        {
            returnType = TokenType.InterfaceDeclaration;
            rule = new TokenType[] {
                TokenType.InterfaceMainPart, TokenType.Whitespace, TokenType.ClassExtends, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                PerformRuleTransform(stack);
                return true;
            }
            return false;
        }

        protected override void PerformRuleTransform(List<Token> stack)
        {
            List<Token> childsInput = new List<Token>();
            childsInput.AddRange(stack[stack.Count - 4].Childs);
            foreach (var cur in stack[stack.Count - 2].Childs)
                childsInput.Add(cur);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

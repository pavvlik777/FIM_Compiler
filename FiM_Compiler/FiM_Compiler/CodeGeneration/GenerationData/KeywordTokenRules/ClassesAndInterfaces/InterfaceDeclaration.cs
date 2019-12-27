using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ClassesAndInterfaces
{
    public class InterfaceDeclaration : TokenRule
    {
        public InterfaceDeclaration()
        {
            returnType = TokenType.InterfaceDeclaration;
            rule = new TokenType[] {
                TokenType.InterfaceMainPart, TokenType.Punctuation
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
            var childsInput = new List<Token>();
            childsInput.AddRange(stack[stack.Count - 2].Childs);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

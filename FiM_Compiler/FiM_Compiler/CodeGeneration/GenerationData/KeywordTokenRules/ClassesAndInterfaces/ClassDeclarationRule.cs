using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ClassesAndInterfaces
{
    public class ClassDeclarationRule : TokenRule
    {
        public ClassDeclarationRule()
        {
            returnType = TokenType.ClassDeclaration;
            rule = new TokenType[] {
                TokenType.ClassMainPart, TokenType.Punctuation, TokenType.Whitespace, TokenType.Name, TokenType.Punctuation
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
            childsInput.AddRange(stack[stack.Count - 5].Childs);
            childsInput.Add(stack[stack.Count - 2]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

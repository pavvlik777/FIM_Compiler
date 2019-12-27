using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ClassesAndInterfaces
{
    public class ClassDeclarationWithExtendsRule : TokenRule
    {
        public ClassDeclarationWithExtendsRule()
        {
            returnType = TokenType.ClassDeclaration;
            rule = new TokenType[] {
                TokenType.ClassMainPart, TokenType.Whitespace, TokenType.ClassExtends, TokenType.Punctuation, TokenType.Whitespace, TokenType.Name, TokenType.Punctuation
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
            childsInput.AddRange(stack[stack.Count - 7].Childs);
            childsInput.Add(stack[stack.Count - 2]);
            foreach (var cur in stack[stack.Count - 5].Childs)
                childsInput.Add(cur);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

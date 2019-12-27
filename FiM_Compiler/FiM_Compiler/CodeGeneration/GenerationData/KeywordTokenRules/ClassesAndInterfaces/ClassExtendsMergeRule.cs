using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ClassesAndInterfaces
{
    public class ClassExtendsMergeRule : TokenRule
    {
        public ClassExtendsMergeRule()
        {
            returnType = TokenType.ClassExtends;
            rule = new TokenType[] {
                TokenType.ClassExtends, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.Name
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.ClassImplements, stack[stack.Count - 3].Value))
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
            foreach (var cur in stack[stack.Count - 3].Childs)
                childsInput.Add(cur);
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

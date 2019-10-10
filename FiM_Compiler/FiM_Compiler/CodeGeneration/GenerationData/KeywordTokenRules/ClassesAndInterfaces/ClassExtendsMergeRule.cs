using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class ClassExtendsMergeRule : TokenRule
    {
        public ClassExtendsMergeRule()
        {
            returnType = TokenType.ClassExtends;
            rule = new TokenType[] {
                TokenType.ClassExtends, TokenType.Whitespace, TokenType.ClassExtends
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                PerformRuleTransform(ref stack);
                return true;
            }
            return false;
        }

        protected override void PerformRuleTransform(ref List<Token> stack)
        {
            List<Token> childsInput = new List<Token>();
            foreach (var cur in stack[stack.Count - 3].Childs)
                childsInput.Add(cur);
            foreach (var cur in stack[stack.Count - 1].Childs)
                childsInput.Add(cur);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

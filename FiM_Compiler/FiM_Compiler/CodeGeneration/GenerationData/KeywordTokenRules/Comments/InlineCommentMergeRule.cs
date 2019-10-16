using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class InlineCommentMergeRule : TokenRule
    {
        public InlineCommentMergeRule()
        {
            returnType = TokenType.InlineCommentStart;
            rule = new TokenType[] {
                TokenType.Name, TokenType.Punctuation, TokenType.InlineCommentStart
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 3].Value == "P" && stack[stack.Count - 2].Value == ".")
                {
                    PerformRuleTransform(stack);
                    return true;
                }
            }
            return false;
        }

        protected override void PerformRuleTransform(List<Token> stack)
        {
            ConvertTokens(ref stack, rule.Length, returnType);
        }
    }
}

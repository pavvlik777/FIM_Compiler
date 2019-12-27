using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.StatementsAndLoops
{
    public class DoWhileStart : TokenRule
    {
        public DoWhileStart()
        {
            returnType = TokenType.DoWhileStart;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.DoWhileStart, stack[stack.Count - 2].Value)
                    && stack[stack.Count - 1].Value == ":")
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

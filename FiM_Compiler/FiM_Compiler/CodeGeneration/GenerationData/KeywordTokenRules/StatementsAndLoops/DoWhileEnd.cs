using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.StatementsAndLoops
{
    public class DoWhileEnd : TokenRule
    {
        public DoWhileEnd()
        {
            returnType = TokenType.DoWhileEnd;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.BoolValue, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.DoWhileEnd, stack[stack.Count - 4].Value)
                    && stack[stack.Count - 1].Value == ".")
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
            childsInput.Add(stack[stack.Count - 2]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

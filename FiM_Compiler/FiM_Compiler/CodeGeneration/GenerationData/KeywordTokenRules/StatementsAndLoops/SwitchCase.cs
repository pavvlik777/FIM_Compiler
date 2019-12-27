using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.StatementsAndLoops
{
    public class SwitchCase : TokenRule
    {
        public SwitchCase()
        {
            returnType = TokenType.SwitchCase;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.Literal, TokenType.Whitespace, TokenType.Keyword, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.SwitchCaseFirst, stack[stack.Count - 6].Value)
                    && KeywordsDictionary.IsKeyword(KeywordType.SwitchCaseSecond, stack[stack.Count - 2].Value)
                    && stack[stack.Count- 1].Value == ":")
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
            childsInput.Add(stack[stack.Count - 4]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
            stack.Insert(stack.Count - 1, new Token(TokenType.SwitchCaseEnd));
        }
    }
}

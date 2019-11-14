using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.StatementsAndLoops
{
    public class SwitchDefaultCase : TokenRule
    {
        public SwitchDefaultCase()
        {
            returnType = TokenType.SwitchDefaultCase;
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
                if (KeywordsDictionary.IsKeyword(KeywordType.SwitchDefaultCase, stack[stack.Count - 2].Value)
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
            stack.Insert(stack.Count - 1, new Token(TokenType.SwitchCaseEnd));
        }
    }
}

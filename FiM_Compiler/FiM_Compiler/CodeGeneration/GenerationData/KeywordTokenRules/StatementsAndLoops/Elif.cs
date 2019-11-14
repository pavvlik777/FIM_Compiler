using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.StatementsAndLoops
{
    public class Elif : TokenRule
    {
        public Elif()
        {
            returnType = TokenType.Elif;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.BoolValue, TokenType.Whitespace, TokenType.Keyword, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.ElifStart, stack[stack.Count - 6].Value)
                    && KeywordsDictionary.IsKeyword(KeywordType.IfStartSecond, stack[stack.Count - 2].Value)
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
            List<Token> childsInput = new List<Token>();
            childsInput.Add(stack[stack.Count - 4]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
            stack.Insert(stack.Count - 1, new Token(TokenType.IfEnd));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.BooleanOperations
{
    public class IsGreaterThan : TokenRule
    {
        public IsGreaterThan()
        {
            returnType = TokenType.GreaterThan;
            rule = new TokenType[] {
                TokenType.Value, TokenType.Whitespace, TokenType.Keyword, TokenType.SingleSpace, TokenType.Keyword, TokenType.Whitespace, TokenType.Value
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.BooleanComparator, stack[stack.Count - 5].Value)
                    && KeywordsDictionary.IsKeyword(KeywordType.BooleanGreaterThan, stack[stack.Count - 3].Value))
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
            childsInput.Add(stack[stack.Count - 7]);
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

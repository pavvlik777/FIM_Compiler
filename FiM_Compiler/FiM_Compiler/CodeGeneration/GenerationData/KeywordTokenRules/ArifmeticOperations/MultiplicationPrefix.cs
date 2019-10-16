using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ArifmeticOperations
{
    public class MultiplicationPrefix : TokenRule
    {
        public MultiplicationPrefix()
        {
            returnType = TokenType.ArifmeticMultiplication;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.IntValue, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.IntValue
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.ArifmeticMultiplicationPrefixFirst, stack[stack.Count - 7].Value)
                    && KeywordsDictionary.IsKeyword(KeywordType.ArifmeticMultiplicationPrefixSecond, stack[stack.Count - 3].Value))
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
            childsInput.Add(stack[stack.Count - 5]);
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

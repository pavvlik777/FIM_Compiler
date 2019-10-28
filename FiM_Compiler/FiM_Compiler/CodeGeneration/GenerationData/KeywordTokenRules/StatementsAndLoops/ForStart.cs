using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.StatementsAndLoops
{
    public class ForStart : TokenRule
    {
        public ForStart() //TODO mb add feature to declare variable in for/foreach start 
        {
            returnType = TokenType.ForStart;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.VariableName, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace,
                TokenType.Value, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.Value, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.ForStartFirst, stack[stack.Count - 12].Value)
                    && KeywordsDictionary.IsKeyword(KeywordType.ForStartSecond, stack[stack.Count - 8].Value)
                    && KeywordsDictionary.IsKeyword(KeywordType.ForStartThird, stack[stack.Count - 4].Value)
                    && stack[stack.Count - 1].Value == ",")
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
            childsInput.Add(stack[stack.Count - 10]);
            childsInput.Add(stack[stack.Count - 6]);
            childsInput.Add(stack[stack.Count - 2]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

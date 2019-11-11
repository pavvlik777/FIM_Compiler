using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.StatementsAndLoops
{
    public class ForStartWithDeclaring : TokenRule
    {
        public ForStartWithDeclaring()
        {
            returnType = TokenType.ForStartWithDeclaring;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.VariableType, TokenType.Whitespace, TokenType.Name, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace,
                TokenType.Value, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.Value, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.ForStartFirst, stack[stack.Count - 14].Value)
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
            childsInput.Add(stack[stack.Count - 12]);
            childsInput.Add(stack[stack.Count - 10]);
            childsInput.Add(stack[stack.Count - 6]);
            childsInput.Add(stack[stack.Count - 2]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

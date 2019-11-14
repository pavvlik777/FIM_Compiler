using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.UserInteractions
{
    public class Prompt : TokenRule
    {
        public Prompt()
        {
            returnType = TokenType.UserPrompt;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.SingleSpace, TokenType.VariableName, TokenType.Whitespace, TokenType.Value, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.UserPrompt, stack[stack.Count - 6].Value))
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
            childsInput.Add(stack[stack.Count - 2]);
            childsInput.Add(stack[stack.Count - 4]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.MethodsActions
{
    public class MethodCallingWithKeyword : TokenRule
    {
        public MethodCallingWithKeyword()
        {
            returnType = TokenType.MethodCalling;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.MethodCalling, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.MethodCalling, stack[stack.Count - 4].Value))
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
            childsInput.AddRange(stack[stack.Count - 2].Childs);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ClassesAndInterfaces
{
    public class ClassExtendsRule : TokenRule
    {
        public ClassExtendsRule()
        {
            returnType = TokenType.ClassExtends;
            rule = new TokenType[] {
                TokenType.ClassMainPart, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.Name
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[]
                {
                    TokenType.InterfaceMainPart, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.Name
                }
            };
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.ClassImplements, stack[stack.Count - 3].Value))
                {
                    PerformRuleTransform(stack);
                    return true;
                }
            }
            foreach (var curRule in variations)
            {
                if (DefaultStackCheck(stack, curRule))
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
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, 3, returnType, childsInput);
        }
    }
}

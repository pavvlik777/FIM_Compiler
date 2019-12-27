using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.MethodsDeclaration
{
    public class MethodParameters : TokenRule
    {
        public MethodParameters()
        {
            returnType = TokenType.MethodParameters;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.VariableType, TokenType.Whitespace, TokenType.Name
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (KeywordsDictionary.IsKeyword(KeywordType.MethodDeclarationParameters, stack[stack.Count - 5].Value))
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
            childsInput.Add(stack[stack.Count - 3]);
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

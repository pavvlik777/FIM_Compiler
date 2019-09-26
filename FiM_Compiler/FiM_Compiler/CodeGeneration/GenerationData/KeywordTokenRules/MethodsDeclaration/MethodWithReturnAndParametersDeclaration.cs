using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class MethodWithReturnAndParametersDeclaration : TokenRule
    {
        public MethodWithReturnAndParametersDeclaration()
        {
            returnType = TokenType.MethodDeclaration;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Whitespace, TokenType.Keyword, TokenType.SingleSpace, TokenType.VariableType, TokenType.Whitespace, TokenType.MethodParameters, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 10].Value == "I learned" && (stack[stack.Count - 6].Value == "with" || stack[stack.Count - 6].Value == "to get"))
                {
                    PerformRuleTransform(ref stack);
                    return true;
                }
            }
            return false;
        }

        protected override void PerformRuleTransform(ref List<Token> stack)
        {
            List<string> childsInput = new List<string>();
            childsInput.Add(stack[stack.Count - 8].Value);
            childsInput.Add(stack[stack.Count - 4].Value);
            childsInput.AddRange(stack[stack.Count - 2].Childs);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

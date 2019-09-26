﻿using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules
{
    public class VariableDeclarationAndAssignValue : TokenRule
    {
        public VariableDeclarationAndAssignValue()
        {
            returnType = TokenType.VariableDeclarationAndAssign;
            rule = new TokenType[] {
                TokenType.Keyword, TokenType.Whitespace, TokenType.Name, TokenType.Whitespace, TokenType.Keyword, TokenType.Whitespace, TokenType.VariableType, TokenType.Whitespace, TokenType.Name, TokenType.Punctuation
            };
            variations = null;
            CheckVariations();
        }

        string[] secondKeyword =
        {
            "is", "was", "has", "had", "like", "likes", "liked"
        };

        bool IsSecond(string input)
        {
            foreach (var cur in secondKeyword)
                if (input == cur)
                    return true;
            return false;
        }

        public override bool IsStackMatch(ref List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 10].Value == "Did you know that" && IsSecond(stack[stack.Count - 6].Value) && stack[stack.Count - 1].Value == "?")
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
            childsInput.Add(stack[stack.Count - 2].Value);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

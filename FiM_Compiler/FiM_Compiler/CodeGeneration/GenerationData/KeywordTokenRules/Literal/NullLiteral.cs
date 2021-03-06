﻿using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Literal
{
    public class NullLiteral : TokenRule
    {
        public NullLiteral()
        {
            returnType = TokenType.NullLiteral;
            rule = new TokenType[] {
                TokenType.Name
            };
            variations = null;
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                if (stack[stack.Count - 1].Value == "nothing")
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
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

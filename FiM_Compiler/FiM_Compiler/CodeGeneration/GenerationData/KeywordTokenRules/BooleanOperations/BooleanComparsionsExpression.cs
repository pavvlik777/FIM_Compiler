using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.BooleanOperations
{
    public class BooleanComparsionsExpression : TokenRule
    {
        public BooleanComparsionsExpression()
        {
            returnType = TokenType.BooleanExpression;
            rule = new TokenType[] {
                TokenType.IsEqual
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[] { TokenType.IsNotEqual },
                new TokenType[] { TokenType.GreaterThan },
                new TokenType[] { TokenType.GreaterThanOrEqual },
                new TokenType[] { TokenType.LessThan },
                new TokenType[] { TokenType.LessThanOrEqual },
            };
            CheckVariations();
        }

        public override bool IsStackMatch(List<Token> stack)
        {
            if (DefaultStackCheck(stack, rule))
            {
                PerformRuleTransform(stack);
                return true;
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
            List<Token> childsInput = new List<Token>();
            childsInput.Add(stack[stack.Count - 5]);
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

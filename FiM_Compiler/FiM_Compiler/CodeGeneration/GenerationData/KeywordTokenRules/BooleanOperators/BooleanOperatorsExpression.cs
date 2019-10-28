using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.BooleanOperations
{
    public class BooleanOperatorsExpression : TokenRule
    {
        public BooleanOperatorsExpression()
        {
            returnType = TokenType.BooleanExpression;
            rule = new TokenType[] {
                TokenType.BooleanAnd
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[] { TokenType.BooleanNot },
                new TokenType[] { TokenType.BooleanOr },
                new TokenType[] { TokenType.BooleanXor },
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
            childsInput.Add(stack[stack.Count - 1]);
            ConvertTokens(ref stack, rule.Length, returnType, childsInput);
        }
    }
}

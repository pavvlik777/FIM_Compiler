using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ArifmeticOperations
{
    public class ArifmeticExpression : TokenRule
    {
        public ArifmeticExpression()
        {
            returnType = TokenType.ArifmeticExpression;
            rule = new TokenType[] {
                TokenType.ArifmeticAddition
            };
            variations = new List<TokenType[]>()
            {
                new TokenType[] { TokenType.ArifmeticDecrement },
                new TokenType[] { TokenType.ArifmeticDivision },
                new TokenType[] { TokenType.ArifmeticIncrement },
                new TokenType[] { TokenType.ArifmeticMultiplication },
                new TokenType[] { TokenType.ArifmeticSubstraction }
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

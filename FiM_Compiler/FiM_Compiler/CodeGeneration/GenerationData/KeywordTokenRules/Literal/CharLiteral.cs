using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Literal
{
    public class CharLiteral : TokenRule
    {
        public CharLiteral()
        {
            returnType = TokenType.CharLiteral;
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
                if (stack[stack.Count - 1].Value.Length == 3)
                { 
                    if(stack[stack.Count - 1].Value[0] == '\'' && stack[stack.Count - 1].Value[2] == '\'')
                    {
                        PerformRuleTransform(stack);
                        return true;
                    }
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

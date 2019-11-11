using System;
using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public abstract class TokenRule
    {
        protected TokenType[] rule;
        protected List<TokenType[]> variations;
        protected TokenType returnType;

        public int Amount
        { get { return rule.Length; } }

        public abstract bool IsStackMatch(List<Token> stack);

        protected abstract void PerformRuleTransform(List<Token> stack);

        protected void CheckVariations()
        {
            if (variations == null) return;
            foreach (var cur in variations)
                if (cur.Length != Amount)
                    throw new InvalidOperationException("Invalid variation of main rule");
        }

        protected bool DefaultStackCheck(List<Token> stack, TokenType[] rule)
        {
            if (stack.Count >= rule.Length)
            {
                return IsPattern(stack, rule);
            }
            return false;
        }

        protected bool IsPattern(List<Token> stack, TokenType[] checks)
        {
            bool output = true;
            if (stack.Count > checks.Length - 1)
            {
                for (int j = 0, i = checks.Length; i >= 1; i--, j = checks.Length - i)
                    if (checks[j] == TokenType.Whitespace)
                    {
                        if (stack[stack.Count - i].Type != TokenType.Whitespace && stack[stack.Count - i].Type != TokenType.SingleSpace)
                        {
                            output = false;
                            break;
                        }
                    }
                //TODO check literals
                    else if (checks[j] == TokenType.Literal)
                    {
                        if (stack[stack.Count - i].Type != TokenType.BoolLiteral && stack[stack.Count - i].Type != TokenType.CharLiteral && stack[stack.Count - i].Type != TokenType.IntLiteral
                             && stack[stack.Count - i].Type != TokenType.NullLiteral && stack[stack.Count - i].Type != TokenType.StringLiteral)
                        {
                            output = false;
                            break;
                        }
                    }
                    else if(checks[j] == TokenType.Value)
                    {
                        if(stack[stack.Count - i].Type != TokenType.BoolLiteral && stack[stack.Count - i].Type != TokenType.CharLiteral && stack[stack.Count - i].Type != TokenType.IntLiteral
                             && stack[stack.Count - i].Type != TokenType.NullLiteral && stack[stack.Count - i].Type != TokenType.StringLiteral /*&& stack[stack.Count - i].Type != TokenType.Name*/
                             && stack[stack.Count - i].Type != TokenType.MethodCalling && stack[stack.Count - i].Type != TokenType.VariableName
                             && stack[stack.Count - i].Type != TokenType.ArifmeticExpression && stack[stack.Count - i].Type != TokenType.BooleanExpression)
                        {
                            output = false;
                            break;
                        }
                    }
                    else if(checks[j] == TokenType.BoolValue)
                    {
                        if(stack[stack.Count - i].Type != TokenType.BoolLiteral && stack[stack.Count - i].Type != TokenType.BooleanExpression
                            && stack[stack.Count - i].Type != TokenType.VariableName && stack[stack.Count - i].Type != TokenType.MethodCalling)
                        {
                            output = false;
                            break;
                        }
                    }
                    else if (checks[j] == TokenType.IntValue)
                    {
                        if (stack[stack.Count - i].Type != TokenType.IntLiteral && stack[stack.Count - i].Type != TokenType.ArifmeticExpression
                            && stack[stack.Count - i].Type != TokenType.VariableName && stack[stack.Count - i].Type != TokenType.MethodCalling)
                        {
                            output = false;
                            break;
                        }
                    }
                    else
                    {
                        if (stack[stack.Count - i].Type != checks[j])
                        {
                            output = false;
                            break;
                        }
                    }
            }
            else
            {
                throw new InvalidOperationException("Stack doesn't have enough elemets for check");
            }
            return output;
        }

        protected void ConvertTokens(ref List<Token> stack, int amount, TokenType newType, List<Token> childs = null)
        {
            string value = "";
            Token newToken = null;
            for (int j = amount; j >= 1; j--)
                value += stack[stack.Count - j].Value;
            if (childs != null)
                newToken = new Token(newType, value, new List<Token>(childs));
            else
                newToken = new Token(newType, value);
            for (int j = 1; j <= amount; j++)
                stack.RemoveAt(stack.Count - 1);
            stack.Add(newToken);
        }
    }
}

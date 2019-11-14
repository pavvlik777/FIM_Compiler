using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public abstract class SyntaxNode
    {
        private SyntaxType type;
        public SyntaxType Type 
        { 
            get
            {
                return type;
            }
        }
        protected string GetExpressionType(Token token, List<Error> compileErrors, Dictionary<string, string> variables)
        {
            return "";
        }
        protected bool CheckTypes(Token token, List<Error> compileErrors, Dictionary<string, string> variables)
        {
            //TODO check types
            return true;
        }
        protected string ParseExpression(Token _token)
        {
            if (_token.Type == TokenType.BooleanExpression)
            {
                return ParseExpression(_token.Childs[0]);
            }
            else if (_token.Type == TokenType.ArifmeticExpression)
            {
                return ParseExpression(_token.Childs[0]);
            }
            else if (_token.Type == TokenType.BoolLiteral)
            {
                if (_token.Value == "yes" || _token.Value == "true" || _token.Value == "right" || _token.Value == "correct")
                    return "true";
                else
                    return "false";
            }
            else if (_token.Type == TokenType.IntLiteral || _token.Type == TokenType.Char || _token.Type == TokenType.StringLiteral)
            {
                return _token.Value;
            }
            else if (_token.Type == TokenType.VariableName)
            {
                return _token.ValueWithoutWhitespaces;
            }
            else if (_token.Type == TokenType.MethodCalling)
            {
                string output = $"{_token.Childs[0].ValueWithoutWhitespaces}(";
                if (_token.Childs.Count >= 2)
                {
                    output += $"{ParseExpression(_token.Childs[1])}";
                    for (int i = 2; i < _token.Childs.Count; i++)
                        output += $", {ParseExpression(_token.Childs[i])}";
                }
                output += ")";
                return output;
            }
            else if (_token.Type == TokenType.NullLiteral)
            {
                return "null";
            }
            else if (_token.Type == TokenType.BooleanAnd)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} && {res1}";
            }
            else if (_token.Type == TokenType.BooleanOr)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} || {res1}";
            }
            else if (_token.Type == TokenType.BooleanXor)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} ^ {res1}";
            }
            else if (_token.Type == TokenType.BooleanNot)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                return $"!{res0}";
            }
            else if (_token.Type == TokenType.IsEqual)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} == {res1}";
            }
            else if (_token.Type == TokenType.IsNotEqual)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} != {res1}";
            }
            else if (_token.Type == TokenType.GreaterThan)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} > {res1}";
            }
            else if (_token.Type == TokenType.GreaterThanOrEqual)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} >= {res1}";
            }
            else if (_token.Type == TokenType.LessThan)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} < {res1}";
            }
            else if (_token.Type == TokenType.LessThanOrEqual)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} <= {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticAddition)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} + {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticSubstraction)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} - {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticMultiplication)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} * {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticDivision)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                string res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} / {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticIncrement)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                return $"{res0}++";
            }
            else if (_token.Type == TokenType.ArifmeticDecrement)
            {
                string res0 = ParseExpression(_token.Childs[0]);
                return $"{res0}--";
            }
            return "";
        }
        public List<SyntaxNode> Nodes { get; set; }

        public abstract string GenerateCode(string offset = "");

        public virtual bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }

        public SyntaxNode(SyntaxType type)
        {
            Nodes = new List<SyntaxNode>();
            this.type = type;
        }
    }
}

using System.Collections.Generic;

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
        protected string GetExpressionType(Token token, List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            if(token.Type == TokenType.BoolLiteral)
            {
                return "bool";
            }
            else if(token.Type == TokenType.CharLiteral)
            {
                return "char";
            }
            else if (token.Type == TokenType.StringLiteral)
            {
                return "string";
            }
            else if (token.Type == TokenType.IntLiteral)
            {
                return "int";
            }
            else if (token.Type == TokenType.NullLiteral)
            {
                return "null";
            }
            else if (token.Type == TokenType.BooleanExpression || token.Type == TokenType.ArifmeticExpression)
            {
                return GetExpressionType(token.Childs[0], compileErrors, variables, methods);
            }
            else if (token.Type == TokenType.BooleanAnd)
            {
                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                if ((value1 != "bool" || value2 != "bool") && (value1 != "null" || value2 != "bool") && (value2 != "null" || value1 != "bool"))
                {
                    compileErrors.Add(new Error("Both parts of boolean xor must have type bool"));
                    return "Error";
                }
                return "bool";
            }
            else if (token.Type == TokenType.BooleanOr)
            {
                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                if ((value1 != "bool" || value2 != "bool") && (value1 != "null" || value2 != "bool") && (value2 != "null" || value1 != "bool"))
                {
                    compileErrors.Add(new Error("Both parts of boolean xor must have type bool"));
                    return "Error";
                }
                return "bool";
            }
            else if (token.Type == TokenType.BooleanXor)
            {
                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                if ((value1 != "bool" || value2 != "bool") && (value1 != "null" || value2 != "bool") && (value2 != "null" || value1 != "bool"))
                {
                    compileErrors.Add(new Error("Both parts of boolean xor must have type bool"));
                    return "Error";
                }
                return "bool";
            }
            else if (token.Type == TokenType.BooleanNot)
            {
                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                if (value1 != "null" && value1 != "bool")
                {
                    compileErrors.Add(new Error("Operand of boolean not must have type bool"));
                    return "Error";
                }
                return "bool";
            }
            else if (token.Type == TokenType.IsEqual || token.Type == TokenType.IsNotEqual ||
                token.Type == TokenType.GreaterThan || token.Type == TokenType.GreaterThanOrEqual ||
                token.Type == TokenType.LessThan || token.Type == TokenType.LessThanOrEqual)
            {
                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                if (value1 != value2 && value1 != "null" && value2 != "null")
                {
                    compileErrors.Add(new Error("Both parts of comparison operator must have same type"));
                    return "Error";
                }
                return "bool";
            }
            else if (token.Type == TokenType.ArifmeticAddition)
            {
                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                if ((value1 != "int" || value2 != "int") && (value1 != "null" || value2 != "int") && (value2 != "null" || value1 != "int"))
                {
                    compileErrors.Add(new Error("Both parts of addition must have type int"));
                    return "Error";
                }
                return "int";
            }
            else if (token.Type == TokenType.ArifmeticSubstraction)
            {
                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                if ((value1 != "int" || value2 != "int") && (value1 != "null" || value2 != "int") && (value2 != "null" || value1 != "int"))
                {
                    compileErrors.Add(new Error("Both parts of substraction must have type int"));
                    return "Error";
                }
                return "int";
            }
            else if (token.Type == TokenType.ArifmeticMultiplication)
            {

                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                if ((value1 != "int" || value2 != "int") && (value1 != "null" || value2 != "int") && (value2 != "null" || value1 != "int"))
                {
                    compileErrors.Add(new Error("Both parts of nultiplication must have type int"));
                    return "Error";
                }
                return "int";
            }
            else if (token.Type == TokenType.ArifmeticDivision)
            {

                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                if ((value1 != "int" || value2 != "int") && (value1 != "null" || value2 != "int") && (value2 != "null" || value1 != "int"))
                {
                    compileErrors.Add(new Error("Both parts of division must have type int"));
                    return "Error";
                }
                return "int";
            }
            else if (token.Type == TokenType.ArifmeticIncrement)
            {
                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                if (value1 != "null" && value1 != "int")
                {
                    compileErrors.Add(new Error("Operand of increment construction must have type int"));
                    return "Error";
                }
                return "int";
            }
            else if (token.Type == TokenType.ArifmeticDecrement)
            {
                var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                if (value1 != "null" && value1 != "int")
                {
                    compileErrors.Add(new Error("Operand of decrement construction must have type int"));
                    return "Error";
                }
                return "int";
            }
            else if (token.Type == TokenType.VariableName)
            {
                foreach(var cur in variables)
                {
                    if (cur.Item1 == token.Value)
                        return cur.Item2;
                }
                compileErrors.Add(new Error($"Variable {token.Value} doesn't exists"));
                return "Error";
            }
            else if (token.Type == TokenType.MethodCalling)
            {
                foreach (var cur in methods)
                {
                    if (cur.Item1 == token.Value)
                        return cur.Item2;
                }
                compileErrors.Add(new Error($"Method {token.Value} doesn't exists"));
                return "Error";
            }
            return "";
        }
        protected string ParseExpression(Token _token)
        {
            if (_token.Type == TokenType.BooleanExpression || _token.Type == TokenType.ArifmeticExpression)
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
                var output = $"{_token.Childs[0].ValueWithoutWhitespaces}(";
                if (_token.Childs.Count >= 2)
                {
                    output += $"{ParseExpression(_token.Childs[1])}";
                    for (var i = 2; i < _token.Childs.Count; i++)
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
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} && {res1}";
            }
            else if (_token.Type == TokenType.BooleanOr)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} || {res1}";
            }
            else if (_token.Type == TokenType.BooleanXor)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} ^ {res1}";
            }
            else if (_token.Type == TokenType.BooleanNot)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                return $"!{res0}";
            }
            else if (_token.Type == TokenType.IsEqual)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} == {res1}";
            }
            else if (_token.Type == TokenType.IsNotEqual)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} != {res1}";
            }
            else if (_token.Type == TokenType.GreaterThan)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} > {res1}";
            }
            else if (_token.Type == TokenType.GreaterThanOrEqual)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} >= {res1}";
            }
            else if (_token.Type == TokenType.LessThan)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} < {res1}";
            }
            else if (_token.Type == TokenType.LessThanOrEqual)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} <= {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticAddition)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} + {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticSubstraction)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} - {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticMultiplication)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} * {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticDivision)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                var res1 = ParseExpression(_token.Childs[1]);
                return $"{res0} / {res1}";
            }
            else if (_token.Type == TokenType.ArifmeticIncrement)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                return $"{res0}++";
            }
            else if (_token.Type == TokenType.ArifmeticDecrement)
            {
                var res0 = ParseExpression(_token.Childs[0]);
                return $"{res0}--";
            }
            return "";
        }
        public List<SyntaxNode> Nodes { get; set; }

        public abstract string GenerateCode(string offset = "");

        public virtual bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }

        public SyntaxNode(SyntaxType type)
        {
            Nodes = new List<SyntaxNode>();
            this.type = type;
        }
    }
}

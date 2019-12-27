using System.Collections.Generic;
using System.Linq;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;

namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public abstract class SyntaxNode// : ISyntaxNode TODO
    {
        public SyntaxType Type { get; }

        //public abstract SyntaxNode Node(SyntaxType parent, int start, int end, List<Token> tokens, List<Error> compileErrors);


        protected static string GetExpressionType(Token token, List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            switch (token.Type)
            {
                case TokenType.BoolLiteral:
                    return "bool";
                case TokenType.CharLiteral:
                    return "char";
                case TokenType.StringLiteral:
                    return "string";
                case TokenType.IntLiteral:
                    return "int";
                case TokenType.NullLiteral:
                    return "null";
                case TokenType.BooleanExpression:
                case TokenType.ArifmeticExpression:
                    return GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                case TokenType.BooleanAnd:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                        if ((value1 == "bool" && value2 == "bool") || (value1 == "null" && value2 == "bool") ||
                            (value2 == "null" && value1 == "bool"))
                        {
                            return "bool";
                        }
                        compileErrors.Add(new Error("Both parts of boolean xor must have type bool"));

                        return "Error";
                    }
                case TokenType.BooleanOr:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                        if ((value1 == "bool" && value2 == "bool") || (value1 == "null" && value2 == "bool") ||
                            (value2 == "null" && value1 == "bool"))
                        {
                            return "bool";
                        }
                        compileErrors.Add(new Error("Both parts of boolean xor must have type bool"));

                        return "Error";
                    }
                case TokenType.BooleanXor:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                        if ((value1 == "bool" && value2 == "bool") || (value1 == "null" && value2 == "bool") ||
                            (value2 == "null" && value1 == "bool"))
                        {
                            return "bool";
                        }
                        compileErrors.Add(new Error("Both parts of boolean xor must have type bool"));

                        return "Error";
                    }
                case TokenType.BooleanNot:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        if (value1 == "null" || value1 == "bool")
                        {
                            return "bool";
                        }
                        compileErrors.Add(new Error("Operand of boolean not must have type bool"));

                        return "Error";
                    }
                case TokenType.IsEqual:
                case TokenType.IsNotEqual:
                case TokenType.GreaterThan:
                case TokenType.GreaterThanOrEqual:
                case TokenType.LessThan:
                case TokenType.LessThanOrEqual:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                        if (value1 == value2 || value1 == "null" || value2 == "null")
                        {
                            return "bool";
                        }
                        compileErrors.Add(new Error("Both parts of comparison operator must have same type"));

                        return "Error";
                    }
                case TokenType.ArifmeticAddition:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                        if ((value1 == "int" && value2 == "int") || (value1 == "null" && value2 == "int") ||
                            (value2 == "null" && value1 == "int"))
                        {
                            return "int";
                        }
                        compileErrors.Add(new Error("Both parts of addition must have type int"));

                        return "Error";
                    }
                case TokenType.ArifmeticSubstraction:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                        if ((value1 == "int" && value2 == "int") || (value1 == "null" && value2 == "int") ||
                            (value2 == "null" && value1 == "int"))
                        {
                            return "int";
                        }
                        compileErrors.Add(new Error("Both parts of subtraction must have type int"));

                        return "Error";
                    }
                case TokenType.ArifmeticMultiplication:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                        if ((value1 == "int" && value2 == "int") || (value1 == "null" && value2 == "int") ||
                            (value2 == "null" && value1 == "int"))
                        {
                            return "int";
                        }
                        compileErrors.Add(new Error("Both parts of multiplication must have type int"));

                        return "Error";
                    }
                case TokenType.ArifmeticDivision:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        var value2 = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                        if ((value1 == "int" && value2 == "int") || (value1 == "null" && value2 == "int") ||
                            (value2 == "null" && value1 == "int"))
                        {
                            return "int";
                        }
                        compileErrors.Add(new Error("Both parts of division must have type int"));

                        return "Error";
                    }
                case TokenType.ArifmeticIncrement:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        if (value1 == "null" || value1 == "int")
                        {
                            return "int";
                        }
                        compileErrors.Add(new Error("Operand of increment construction must have type int"));

                        return "Error";
                    }
                case TokenType.ArifmeticDecrement:
                    {
                        var value1 = GetExpressionType(token.Childs[0], compileErrors, variables, methods);
                        if (value1 == "null" || value1 == "int")
                        {
                            return "int";
                        }
                        compileErrors.Add(new Error("Operand of decrement construction must have type int"));

                        return "Error";
                    }
                case TokenType.VariableName:
                    {
                        foreach (var cur in variables.Where(cur => cur.Item1 == token.Value))
                        {
                            return cur.Item2;
                        }
                        compileErrors.Add(new Error($"Variable {token.Value} doesn't exists"));

                        return "Error";
                    }
                case TokenType.MethodCalling:
                    {
                        foreach (var cur in methods.Where(cur => cur.Item1 == token.Value))
                        {
                            return cur.Item2;
                        }
                        compileErrors.Add(new Error($"Method {token.Value} doesn't exists"));

                        return "Error";
                    }
                default:
                    {
                        return "";
                    }
            }
        }

        protected static string ParseExpression(Token token)
        {
            switch (token.Type)
            {
                case TokenType.BooleanExpression:
                case TokenType.ArifmeticExpression:
                {
                    return ParseExpression(token.Childs[0]);
                }
                case TokenType.BoolLiteral when token.Value == "yes" || token.Value == "true" || token.Value == "right" || token.Value == "correct":
                {
                    return "true";
                }
                case TokenType.BoolLiteral:
                {
                    return "false";
                }
                case TokenType.IntLiteral:
                case TokenType.Char:
                case TokenType.StringLiteral:
                {
                    return token.Value;
                }
                case TokenType.VariableName:
                {
                    return token.ValueWithoutWhitespaces;
                }
                case TokenType.MethodCalling:
                {
                    var output = $"{token.Childs[0].ValueWithoutWhitespaces}(";
                    if (token.Childs.Count >= 2)
                    {
                        output += $"{ParseExpression(token.Childs[1])}";
                        for (var i = 2; i < token.Childs.Count; i++)
                        {
                            output += $", {ParseExpression(token.Childs[i])}";
                        }
                    }
                    output += ")";

                    return output;
                }
                case TokenType.NullLiteral:
                {
                    return "null";
                }
                case TokenType.BooleanAnd:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} && {res1}";
                }
                case TokenType.BooleanOr:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} || {res1}";
                }
                case TokenType.BooleanXor:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} ^ {res1}";
                }
                case TokenType.BooleanNot:
                {
                    var res0 = ParseExpression(token.Childs[0]);

                    return $"!{res0}";
                }
                case TokenType.IsEqual:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} == {res1}";
                }
                case TokenType.IsNotEqual:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} != {res1}";
                }
                case TokenType.GreaterThan:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} > {res1}";
                }
                case TokenType.GreaterThanOrEqual:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} >= {res1}";
                }
                case TokenType.LessThan:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} < {res1}";
                }
                case TokenType.LessThanOrEqual:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} <= {res1}";
                }
                case TokenType.ArifmeticAddition:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} + {res1}";
                }
                case TokenType.ArifmeticSubstraction:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} - {res1}";
                }
                case TokenType.ArifmeticMultiplication:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} * {res1}";
                }
                case TokenType.ArifmeticDivision:
                {
                    var res0 = ParseExpression(token.Childs[0]);
                    var res1 = ParseExpression(token.Childs[1]);

                    return $"{res0} / {res1}";
                }
                case TokenType.ArifmeticIncrement:
                {
                    var res0 = ParseExpression(token.Childs[0]);

                    return $"{res0}++";
                }
                case TokenType.ArifmeticDecrement:
                {
                    var res0 = ParseExpression(token.Childs[0]);

                    return $"{res0}--";
                }
                default:
                {
                    return "";
                }
            }
        }
        public List<SyntaxNode> Nodes { get; private set; }

        public abstract string GenerateCode(string offset = "");

        public virtual bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var status = true;
            foreach (var cur in Nodes)
            {
                status = status && cur.CheckNode(compileErrors, variables, methods);
            }
            return status;
        }


        public SyntaxNode(SyntaxType type)
        {
            Nodes = new List<SyntaxNode>();
            Type = type;
        }
    }
}

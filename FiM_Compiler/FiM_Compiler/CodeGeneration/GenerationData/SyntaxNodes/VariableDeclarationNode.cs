using System.Collections.Generic;
using System.Linq;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class VariableDeclarationNode : SyntaxNode
    {
        private Token token;
        public override string GenerateCode(string offset = "")
        {
            if(token.Childs[1].Type == TokenType.VariableType)
            {
                return $"{offset}{token.Childs[1].VariableTypeValue} {token.Childs[0].ValueWithoutWhitespaces};\n";
            }
            else
            {
                return $"{offset}var {token.Childs[0].ValueWithoutWhitespaces} = {ParseExpression(token.Childs[1])};\n";
            }
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            if (token.Childs[1].Type == TokenType.VariableType)
            {
                if(variables.Any(x => x.Item1 == token.Childs[0].Value))
                {
                    compileErrors.Add(new Error($"Variable with name {token.Childs[0].Value} already exists"));
                    return false;
                }
                variables.Add((token.Childs[0].Value, token.Childs[1].VariableTypeValue));
            }
            else
            {
                if (variables.Any(x => x.Item1 == token.Childs[0].Value))
                {
                    compileErrors.Add(new Error($"Variable with name {token.Childs[0].Value} already exists"));
                    return false;
                }
                var type = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
                if (type == "Error")
                    return false;
                else if(type == "null" || type == "void")
                {
                    compileErrors.Add(new Error($"Variable with name {token.Childs[0].Value} must be assigned with value of correct type"));
                    return false;
                }
                variables.Add((token.Childs[0].Value, type));
            }
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }
        public VariableDeclarationNode(Token token) : base (SyntaxType.VariableDeclaration)
        {
            this.token = token;
        }
    }
}

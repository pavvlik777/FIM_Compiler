using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class VariableAssignNode : SyntaxNode
    {
        private Token token;
        public override string GenerateCode(string offset = "")
        {
            return $"{offset}{token.Childs[0].ValueWithoutWhitespaces} = {ParseExpression(token.Childs[1])};\n";
        }

        public VariableAssignNode(Token token) : base(SyntaxType.VariableAssign)
        {
            this.token = token;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var type = GetExpressionType(token.Childs[1], compileErrors, variables, methods);
            if (type == "Error")
                return false;
            else if (type == "null" || type == "void")
            {
                compileErrors.Add(new Error($"Variable with name {token.Childs[0].Value} must be assigned with value of correct type"));
                return false;
            }
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }
    }
}

using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class IfTrueNode : SyntaxNode
    {
        private Token token;
        public override string GenerateCode(string offset = "")
        {
            var code = $"{offset}if ({ParseExpression(token)})";
            code += " {\n";
            for(var i = 1; i < Nodes.Count; i++)
                code += Nodes[i].GenerateCode(offset + "\t");
            code += $"{offset}}}\n";

            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var type = GetExpressionType(token, compileErrors, variables, methods);
            if (type == "Error")
                return false;
            else if (type != "bool")
            {
                compileErrors.Add(new Error($"Condition in if statement must have type bool"));
                return false;
            }
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }

        public IfTrueNode(Token token) : base(SyntaxType.IfTruePart)
        {
            this.token = token;
        }
    }
}

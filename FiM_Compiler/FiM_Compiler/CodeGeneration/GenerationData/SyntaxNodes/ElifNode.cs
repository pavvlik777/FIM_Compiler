using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class ElifNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            var code = $"{offset}else if ({ParseExpression(token)})";
            code += " {\n";
            for (var i = 1; i < Nodes.Count; i++)
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
                compileErrors.Add(new Error($"Condition in else-if statement must have type bool"));
                return false;
            }
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }
        public ElifNode(Token token) : base(SyntaxType.ElifPart)
        {
            this.token = token;
        }
    }
}

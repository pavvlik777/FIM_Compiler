using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class IfTrueNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            string code = $"{offset}if ({ParseExpression(token)})";
            code += " {\n";
            for(int i = 1; i < Nodes.Count; i++)
                code += Nodes[i].GenerateCode(offset + "\t");
            code += $"{offset}}}\n";

            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            string type = GetExpressionType(token, compileErrors, variables, methods);
            if (type == "Error")
                return false;
            else if (type != "bool")
            {
                compileErrors.Add(new Error($"Condition in if statement must have type bool"));
                return false;
            }
            bool status = true;
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

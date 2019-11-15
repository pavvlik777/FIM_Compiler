using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class IfTrueNode : SyntaxNode
    {
        public override string GenerateCode(string offset = "")
        {
            string code = $"{offset}if ({Nodes[0].GenerateCode()})";
            code += " {\n";
            for(int i = 1; i < Nodes.Count; i++)
                code += Nodes[i].GenerateCode(offset + "\t");
            code += $"{offset}}}\n";

            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }

        public IfTrueNode() : base(SyntaxType.IfTruePart)
        {

        }
    }
}

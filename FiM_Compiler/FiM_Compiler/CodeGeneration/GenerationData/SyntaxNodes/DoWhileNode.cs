using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class DoWhileNode : SyntaxNode
    {
        public override string GenerateCode(string offset = "")
        {
            string code = $"{offset}do {{\n";
            for(int i = 1; i < Nodes.Count; i++)
                code += Nodes[i].GenerateCode(offset + "\t");
            code += $"{offset}}}\n";
            code += $"{offset}while ({Nodes[0].GenerateCode()});\n";
            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            int amountOfVars = variables.Count;
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            while (variables.Count != amountOfVars)
                variables.RemoveAt(variables.Count - 1);
            return status;
        }

        public DoWhileNode() : base(SyntaxType.DoWhile)
        {

        }
    }
}

using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class IfElseNode : SyntaxNode
    {        
        public override string GenerateCode(string offset = "")
        {
            var code = $"{offset}else";
            code += " {\n\n";
            foreach (var cur in Nodes)
                code += cur.GenerateCode(offset + "\t");
            code += $"{offset}}}\n";

            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }

        public IfElseNode() : base(SyntaxType.IfFalsePart)
        {
        }
    }
}

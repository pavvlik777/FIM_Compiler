using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class InterfaceMethodNode : SyntaxNode
    {
        Token start;
        public override string GenerateCode(string offset = "")
        {
            var code = $"{offset}{start.Childs[1].VariableTypeValue} {start.Childs[0].ValueWithoutWhitespaces} (";
            if (start.Childs.Count >= 3)
            {
                code += $"{start.Childs[2].VariableTypeValue} {start.Childs[3].ValueWithoutWhitespaces}";
                for (var i = 4; i < start.Childs.Count; i += 2)
                    code += $", {start.Childs[i].VariableTypeValue} {start.Childs[i + 1].ValueWithoutWhitespaces}";
            }
            code += ");\n";

            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }

        public InterfaceMethodNode(Token start) : base(SyntaxType.InterfaceMethodDeclaring)
        {
            this.start = start;
        }
    }
}

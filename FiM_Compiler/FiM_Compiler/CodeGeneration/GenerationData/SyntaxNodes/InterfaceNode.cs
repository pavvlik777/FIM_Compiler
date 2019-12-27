using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class InterfaceNode : SyntaxNode
    {
        Token start, end;
        public override string GenerateCode(string offset = "")
        {
            var code = $"interface {start.Childs[0].ValueWithoutWhitespaces}";
            if(start.Childs.Count >= 2)
            {
                code += $" : {start.Childs[1].ValueWithoutWhitespaces}";
                for (var i = 2; i < start.Childs.Count; i++)
                    code += $", {start.Childs[i].ValueWithoutWhitespaces}";
            }
            code += "\n{\n";
            foreach (var cur in Nodes)
                code += cur.GenerateCode(offset + "\t");
            code += @"} //";
            code += $"{end.Childs[0].Value}";
            code += "\n\n";
            return code;
        }
        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }

        public InterfaceNode(Token start, Token end) : base(SyntaxType.Interface)
        {
            this.start = start;
            this.end = end;
        }
    }
}

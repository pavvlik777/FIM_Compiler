using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class SwitchNode : SyntaxNode
    {
        Token start, end;
        public override string GenerateCode(string offset = "")
        {
            string code = "";
            code += $"{offset}switch({ParseExpression(start.Childs[0])}) {{\n";
            foreach (var cur in Nodes)
                code += cur.GenerateCode("\t" + offset);
            code += $"{offset}}}\n";
            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }

        public SwitchNode(Token start, Token end) : base(SyntaxType.Switch)
        {
            this.start = start;
            this.end = end;
        }
    }
}

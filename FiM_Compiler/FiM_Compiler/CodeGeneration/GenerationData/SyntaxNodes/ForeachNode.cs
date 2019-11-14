using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class ForeachNode : SyntaxNode
    {
        Token start, end;
        public override string GenerateCode(string offset = "")
        {
            string code = $"{offset}foreach(";
            if(start.Childs.Count == 3) //with declaring
            {
                code += $"{start.Childs[0].VariableTypeValue} {start.Childs[1].ValueWithoutWhitespaces} in {ParseExpression(start.Childs[2])}) {{\n";
            }
            else
            {
                code += $"{start.Childs[0].ValueWithoutWhitespaces} in {ParseExpression(start.Childs[1])}) {{\n";
            }
            foreach (var cur in Nodes)
                code += cur.GenerateCode(offset + "\t");
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

        public ForeachNode(Token start, Token end) : base(SyntaxType.Foreach)
        {
            this.start = start;
            this.end = end;
        }
    }
}

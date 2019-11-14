using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class ForNode : SyntaxNode
    {
        Token start, end;
        public override string GenerateCode(string offset = "")
        {
            string code = $"{offset}for(";
            if(start.Childs.Count == 4) //with declaring
            {
                code += $"{start.Childs[0].VariableTypeValue} {ParseExpression(start.Childs[1])} = {ParseExpression(start.Childs[2])}; {start.Childs[1].ValueWithoutWhitespaces} < {ParseExpression(start.Childs[3])}; {start.Childs[1].ValueWithoutWhitespaces}++) {{\n";
            }
            else
            {
                code += $"{start.Childs[0].ValueWithoutWhitespaces} = {ParseExpression(start.Childs[1])}; {start.Childs[0].ValueWithoutWhitespaces} < {ParseExpression(start.Childs[2])}; {start.Childs[0].ValueWithoutWhitespaces}++) {{\n";
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

        public ForNode(Token start, Token end) : base(SyntaxType.For)
        {
            this.start = start;
            this.end = end;
        }
    }
}

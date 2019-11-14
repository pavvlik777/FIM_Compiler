using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class MethodNode : SyntaxNode
    {
        Token start, end;
        bool isMain;
        public override string GenerateCode(string offset = "")
        {
            string code = "";
            if(isMain)
            {
                code += $"{offset}public static {start.Childs[1].VariableTypeValue} Main (";
            }
            else
            {
                code += $"{offset}{start.Childs[1].VariableTypeValue} {start.Childs[0].ValueWithoutWhitespaces} (";
            }
            if(start.Childs.Count >= 3)
            {
                code += $"{start.Childs[2].VariableTypeValue} {start.Childs[3].ValueWithoutWhitespaces}";
                for(int i = 4; i < start.Childs.Count; i += 2)
                    code += $", {start.Childs[i].VariableTypeValue} {start.Childs[i + 1].ValueWithoutWhitespaces}";
            }
            code += ")";
            if(isMain)
            {
                code += @" //";
                code += $"{start.Childs[0].ValueWithoutWhitespaces}";
            }
            code += $"\n{offset}{{\n";

            foreach (var cur in Nodes)
                code += cur.GenerateCode(offset + "\t");

            code += $"\n{offset}}}\n";

            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }

        public MethodNode(Token start, Token end, bool isMain) : base(SyntaxType.MethodDeclaring)
        {
            this.start = start;
            this.end = end;
            this.isMain = isMain;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class ClassNode : SyntaxNode
    {
        Token start, end;
        public override string GenerateCode(string offset = "")
        {
            string code = $"class {start.Childs[1].ValueWithoutWhitespaces} : {start.Childs[0].ValueWithoutWhitespaces}";
            for (int i = 2; i < start.Childs.Count; i++)
                code += $", {start.Childs[i].ValueWithoutWhitespaces}";
            code += "\n{\n";
            foreach (var cur in Nodes)
                code += cur.GenerateCode("\t");
            code += @"} //";
            code += $"{end.Childs[0].Value}";
            code += "\n\n";
            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }

        public ClassNode(Token start, Token end) : base(SyntaxType.Class)
        {
            this.start = start;
            this.end = end;
        }
    }
}

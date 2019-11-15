using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class SwitchCase : SyntaxNode
    {
        Token token;
        bool isDefaultCase;

        public override string GenerateCode(string offset = "")
        {
            string code = "";
            if (isDefaultCase)
                code += $"{offset}default: {{\n";
            else
                code += $"{offset}case {ParseExpression(token.Childs[0])}: {{\n";
            foreach (var cur in Nodes)
                code += cur.GenerateCode("\t" + offset);
            code += $"{offset}break;\n{offset}}}\n";
            return code;
        }

        public SwitchCase(Token token, bool isDefaultCase) : base(SyntaxType.SwitchCase)
        {
            this.token = token;
            this.isDefaultCase = isDefaultCase;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class MethodCalling : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            string output = $"{offset}{token.Childs[0].ValueWithoutWhitespaces}(";
            if (token.Childs.Count >= 2)
            {
                output += $"{ParseExpression(token.Childs[1])}";
                for (int i = 2; i < token.Childs.Count; i++)
                    output += $", {ParseExpression(token.Childs[i])}";
            }
            output += ")\n";
            return output;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }
        public MethodCalling(Token token) : base(SyntaxType.MethodCalling)
        {
            this.token = token;
        }
    }
}

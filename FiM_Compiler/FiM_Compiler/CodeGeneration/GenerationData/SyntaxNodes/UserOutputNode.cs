using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class UserOutputNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            return $"{offset}System.Console.WriteLine({ParseExpression(token.Childs[0])});\n";
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }

        public UserOutputNode(Token token) : base(SyntaxType.UserOutput)
        {
            this.token = token;
        }
    }
}

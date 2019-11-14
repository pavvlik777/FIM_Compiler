using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class MethodReturn : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            string output = $"{offset}return {ParseExpression(token.Childs[0])};";
            return output;
        }
        public MethodReturn(Token token) : base(SyntaxType.MethodCalling)
        {
            this.token = token;
        }

        public override bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }
    }
}

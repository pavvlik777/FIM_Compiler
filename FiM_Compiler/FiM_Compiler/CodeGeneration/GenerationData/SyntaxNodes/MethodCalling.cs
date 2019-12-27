using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class MethodCalling : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            var output = $"{offset}{token.Childs[0].ValueWithoutWhitespaces}(";
            if (token.Childs.Count >= 2)
            {
                output += $"{ParseExpression(token.Childs[1])}";
                for (var i = 2; i < token.Childs.Count; i++)
                    output += $", {ParseExpression(token.Childs[i])}";
            }
            output += ")\n";
            return output;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var status = true;
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

using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class UserPromptNode : SyntaxNode
    {
        private Token token;
        public override string GenerateCode(string offset = "")
        {
            var code = $"{offset}System.Console.WriteLine({ParseExpression(token.Childs[0])});\n";
            code += $"{offset}{token.Childs[1].ValueWithoutWhitespaces} = System.Console.ReadLine();\n";
            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }

        public UserPromptNode(Token token) : base(SyntaxType.UserPrompt)
        {
            this.token = token;
        }
    }
}

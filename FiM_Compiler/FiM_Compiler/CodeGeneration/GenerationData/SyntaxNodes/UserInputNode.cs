using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class UserInputNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            return $"{offset}{token.Childs[0].ValueWithoutWhitespaces} = System.Console.ReadLine();\n";
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }

        public UserInputNode(Token token) : base(SyntaxType.UserInput)
        {
            this.token = token;
        }
    }
}

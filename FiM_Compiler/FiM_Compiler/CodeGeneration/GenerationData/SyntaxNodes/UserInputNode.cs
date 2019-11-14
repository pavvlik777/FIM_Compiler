using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class UserInputNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            return $"{offset}{token.Childs[0].ValueWithoutWhitespaces} = System.Console.ReadLine();\n";
        }

        public override bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }

        public UserInputNode(Token token) : base(SyntaxType.UserInput)
        {
            this.token = token;
        }
    }
}

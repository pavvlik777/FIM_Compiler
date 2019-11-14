using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class VariableDeclarationNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            if(token.Childs[1].Type == TokenType.VariableType)
            {
                return $"{offset}{token.Childs[1].VariableTypeValue} {token.Childs[0].ValueWithoutWhitespaces};\n";
            }
            else
            {
                return $"{offset}var {token.Childs[0].ValueWithoutWhitespaces} = {ParseExpression(token.Childs[1])};\n";
            }
        }

        public override bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }
        public VariableDeclarationNode(Token token) : base (SyntaxType.VariableDeclaration)
        {
            this.token = token;
        }
    }
}

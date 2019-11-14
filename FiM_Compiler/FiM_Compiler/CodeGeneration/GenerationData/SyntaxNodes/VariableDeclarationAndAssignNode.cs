using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class VariableDeclarationAndAssignNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            return $"{offset}{token.Childs[1].VariableTypeValue} {token.Childs[0].ValueWithoutWhitespaces} = {ParseExpression(token.Childs[2])};\n";
        }
        public VariableDeclarationAndAssignNode(Token token) : base(SyntaxType.VariableDeclarationAndAssign)
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

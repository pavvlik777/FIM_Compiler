using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class ExpressionNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            return ParseExpression(token);
        }

        public override bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }

        public ExpressionNode(Token token) : base(SyntaxType.Expression)
        {
            this.token = token;
        }
    }
}

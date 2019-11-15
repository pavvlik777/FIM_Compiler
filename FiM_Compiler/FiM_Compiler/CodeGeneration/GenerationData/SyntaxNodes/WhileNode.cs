using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class WhileNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            string code = $"{offset}while ({ParseExpression(token)})";
            code += " {\n";
            for(int i = 1; i < Nodes.Count; i++)
                code += Nodes[i].GenerateCode(offset + "\t");
            code += $"{offset}}}\n";

            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            string type = GetExpressionType(token, compileErrors, variables, methods);
            if (type == "Error")
                return false;
            else if (type != "bool")
            {
                compileErrors.Add(new Error($"Condition in while statement must have type bool"));
                return false;
            }
            int amountOfVars = variables.Count;
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            while (variables.Count != amountOfVars)
                variables.RemoveAt(variables.Count - 1);
            return status;
        }

        public WhileNode(Token token) : base(SyntaxType.While)
        {
            this.token = token;
        }
    }
}

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

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            if (variables.Any(x => x.Item1 == token.Childs[0].Value))
            {
                compileErrors.Add(new Error($"Variable with name {token.Childs[0].Value} already exists"));
                return false;
            }
            string value = GetExpressionType(token.Childs[2], compileErrors, variables, methods);
            if (value == "Error")
            {
                return false;
            }
            if (value != token.Childs[1].VariableTypeValue && value != "null")
            {
                compileErrors.Add(new Error($"Variable with name {token.Childs[0].Value} must be assigned with value of correct type"));
                return false;
            }
            variables.Add((token.Childs[0].Value, token.Childs[1].VariableTypeValue));
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }
    }
}

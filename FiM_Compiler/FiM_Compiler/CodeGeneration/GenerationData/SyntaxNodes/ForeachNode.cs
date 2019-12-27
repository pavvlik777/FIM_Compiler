using System.Collections.Generic;
using System.Linq;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class ForeachNode : SyntaxNode
    {
        Token start, end;
        public override string GenerateCode(string offset = "")
        {
            var code = $"{offset}foreach(";
            if(start.Childs.Count == 3) //with declaring
            {
                code += $"{start.Childs[0].VariableTypeValue} {start.Childs[1].ValueWithoutWhitespaces} in {ParseExpression(start.Childs[2])}) {{\n";
            }
            else
            {
                code += $"{start.Childs[0].ValueWithoutWhitespaces} in {ParseExpression(start.Childs[1])}) {{\n";
            }
            foreach (var cur in Nodes)
                code += cur.GenerateCode(offset + "\t");
            code += $"{offset}}}\n";
            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var amountOfVars = variables.Count;
            if (start.Childs.Count == 3)
            {
                if (variables.Any(x => x.Item1 == start.Childs[1].Value))
                {
                    compileErrors.Add(new Error($"Variable with name {start.Childs[1].Value} already exists"));
                    return false;
                }
                variables.Add((start.Childs[1].Value, start.Childs[0].VariableTypeValue));
            }
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            while (variables.Count != amountOfVars)
                variables.RemoveAt(variables.Count - 1);
            return status;
        }

        public ForeachNode(Token start, Token end) : base(SyntaxType.Foreach)
        {
            this.start = start;
            this.end = end;
        }
    }
}

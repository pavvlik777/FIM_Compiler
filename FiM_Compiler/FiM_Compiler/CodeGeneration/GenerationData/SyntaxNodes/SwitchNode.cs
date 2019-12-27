using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class SwitchNode : SyntaxNode
    {
        private Token start, end;
        public override string GenerateCode(string offset = "")
        {
            var code = "";
            code += $"{offset}switch({ParseExpression(start.Childs[0])}) {{\n";
            foreach (var cur in Nodes)
                code += cur.GenerateCode("\t" + offset);
            code += $"{offset}}}\n";
            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var isCaseExists = false;
            foreach (var cur in Nodes)
                if (cur.Type == SyntaxType.SwitchCase)
                    isCaseExists = true;
            if(!isCaseExists)
            {
                compileErrors.Add(new Error("Switch statement must have at least one case"));
                return false;
            }
            var amountOfVars = variables.Count;
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            while (variables.Count != amountOfVars)
                variables.RemoveAt(variables.Count - 1);
            return status;
        }

        public SwitchNode(Token start, Token end) : base(SyntaxType.Switch)
        {
            this.start = start;
            this.end = end;
        }
    }
}

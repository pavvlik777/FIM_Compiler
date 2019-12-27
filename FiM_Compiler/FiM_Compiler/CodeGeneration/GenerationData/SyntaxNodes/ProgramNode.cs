using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class ProgramNode : SyntaxNode
    {
        public override string GenerateCode(string offset = "")
        {
            var code = "";
            foreach (var cur in Nodes)
                code += cur.GenerateCode();
            return code;
        }

        public ProgramNode() : base(SyntaxType.Program)
        { }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var amountOfEntryPoints = 0;
            foreach (var cur in Nodes)
                if(cur.Type == SyntaxType.Class)
                {
                    var node = (ClassNode)cur;
                    amountOfEntryPoints += node.AmountOfEntryPoints();
                }
            if(amountOfEntryPoints != 1)
            {
                compileErrors.Add(new Error("Incorrect amount of entry points"));
                return false;
            }

            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }
    }
}

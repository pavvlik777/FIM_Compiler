using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class ProgramNode : SyntaxNode
    {
        public override string GenerateCode(string offset = "")
        {
            string code = "";
            foreach (var cur in Nodes)
                code += cur.GenerateCode();
            return code;
        }

        public ProgramNode() : base(SyntaxType.Program)
        { }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            int amountOfEntryPoints = 0;
            foreach (var cur in Nodes)
                if(cur.Type == SyntaxType.Class)
                {
                    ClassNode node = (ClassNode)cur;
                    amountOfEntryPoints += node.AmountOfEntryPoints();
                }
            if(amountOfEntryPoints != 1)
            {
                compileErrors.Add(new Error("Incorrect amount of entry points"));
                return false;
            }

            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }
    }
}

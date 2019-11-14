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

        public override bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }
    }
}

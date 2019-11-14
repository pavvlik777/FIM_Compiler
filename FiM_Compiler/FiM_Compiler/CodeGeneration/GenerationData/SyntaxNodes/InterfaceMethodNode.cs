﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class InterfaceMethodNode : SyntaxNode
    {
        Token start;
        public override string GenerateCode(string offset = "")
        {
            string code = $"{offset}{start.Childs[1].VariableTypeValue} {start.Childs[0].ValueWithoutWhitespaces} (";
            if (start.Childs.Count >= 3)
            {
                code += $"{start.Childs[2].VariableTypeValue} {start.Childs[3].ValueWithoutWhitespaces}";
                for (int i = 4; i < start.Childs.Count; i += 2)
                    code += $", {start.Childs[i].VariableTypeValue} {start.Childs[i + 1].ValueWithoutWhitespaces}";
            }
            code += ");\n";

            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, Dictionary<string, string> variables)
        {
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables);
            return status;
        }

        public InterfaceMethodNode(Token start) : base(SyntaxType.InterfaceMethodDeclaring)
        {
            this.start = start;
        }
    }
}

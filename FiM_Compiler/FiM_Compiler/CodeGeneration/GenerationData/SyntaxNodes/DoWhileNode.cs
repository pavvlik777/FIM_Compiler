﻿using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class DoWhileNode : SyntaxNode
    {
        Token token;
        public override string GenerateCode(string offset = "")
        {
            var code = $"{offset}do {{\n";
            for(var i = 1; i < Nodes.Count; i++)
                code += Nodes[i].GenerateCode(offset + "\t");
            code += $"{offset}}}\n";
            code += $"{offset}while ({ParseExpression(token)});\n";
            return code;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var type = GetExpressionType(token, compileErrors, variables, methods);
            if (type == "Error")
                return false;
            else if (type != "bool")
            {
                compileErrors.Add(new Error($"Condition in do-while statement must have type bool"));
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

        public DoWhileNode(Token token) : base(SyntaxType.DoWhile)
        {
            this.token = token;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class MethodNode : SyntaxNode
    {
        Token start, end;
        bool isMain;
        public bool IsMain { get { return isMain; } }
        public override string GenerateCode(string offset = "")
        {
            string code = "";
            if(isMain)
            {
                code += $"{offset}public static {start.Childs[1].VariableTypeValue} Main (";
            }
            else
            {
                code += $"{offset}{start.Childs[1].VariableTypeValue} {start.Childs[0].ValueWithoutWhitespaces} (";
            }
            if(start.Childs.Count >= 3)
            {
                code += $"{start.Childs[2].VariableTypeValue} {start.Childs[3].ValueWithoutWhitespaces}";
                for(int i = 4; i < start.Childs.Count; i += 2)
                    code += $", {start.Childs[i].VariableTypeValue} {start.Childs[i + 1].ValueWithoutWhitespaces}";
            }
            code += ")";
            if(isMain)
            {
                code += @" //";
                code += $"{start.Childs[0].ValueWithoutWhitespaces}";
            }
            code += $"\n{offset}{{\n";

            foreach (var cur in Nodes)
                code += cur.GenerateCode(offset + "\t");

            code += $"\n{offset}}}\n";

            return code;
        }

        public (string, string) GetMethodMetadata()
        {
            return (start.Childs[0].Value, start.Childs[1].VariableTypeValue);
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            if(start.Childs[0].Value != end.Childs[0].Value)
            {
                compileErrors.Add(new Error("Method declaration must have similar names in both declaring parts"));
                return false;
            }
            if(start.Childs[1].Value != "void")
            {
                bool isReturnExists = false;
                string targetType = KeywordsDictionary.GetVariableType(start.Childs[1].Value);
                foreach(var cur in Nodes)
                {
                    if(cur.Type == SyntaxType.MethodReturn)
                    {
                        isReturnExists = true;
                        MethodReturn node = (MethodReturn)cur;
                        string value = node.GetReturnType(compileErrors, variables, methods);
                        if(value == "Error")
                        {
                            return false;
                        }
                        if (value != targetType && value != "null")
                        {
                            compileErrors.Add(new Error($"Incorrect return type in method {start.Childs[0].ValueWithoutWhitespaces}"));
                            return false;
                        }
                    }
                }
                if(!isReturnExists)
                {
                    compileErrors.Add(new Error("Method with not void return type must have return expression"));
                    return false;
                }
            }
            int amountOfVars = variables.Count;
            bool status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            while(variables.Count != amountOfVars)
                variables.RemoveAt(variables.Count - 1);
            return status;
        }

        public MethodNode(Token start, Token end, bool isMain) : base(SyntaxType.MethodDeclaring)
        {
            this.start = start;
            this.end = end;
            this.isMain = isMain;
        }
    }
}
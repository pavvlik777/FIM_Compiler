using System.Collections.Generic;
using System.Linq;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class ClassNode : SyntaxNode
    {
        private readonly Token _start;
        private readonly Token _end;


        public override string GenerateCode(string offset = "")
        {
            var code = $"class {_start.Childs[1].ValueWithoutWhitespaces} : {_start.Childs[0].ValueWithoutWhitespaces}";
            for (var i = 2; i < _start.Childs.Count; i++)
            {
                code += $", {_start.Childs[i].ValueWithoutWhitespaces}";
            }
            code += "\n{\n";
            code = Nodes.Aggregate(code, (current, cur) => current + cur.GenerateCode("\t"));
            code += @"} //";
            code += $"{_end.Childs[0].Value}";
            code += "\n\n";
            return code;
        }

        public int AmountOfEntryPoints()
        {
            var amount = 0;
            foreach (var cur in Nodes)
            {
                if (cur.Type != SyntaxType.MethodDeclaring)
                {
                    continue;
                }
                var node = (MethodNode)cur;
                if (node.IsMain) amount++;
            }
            return amount;
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var amountOfMethods = methods.Count;
            foreach(var cur in Nodes)
            {
                if (cur.Type != SyntaxType.MethodDeclaring)
                {
                    continue;
                }
                var node = (MethodNode)cur;
                var metadata = node.GetMethodMetadata();
                if (methods.Any(x => x.Item1 == metadata.Item1))
                {
                    compileErrors.Add(new Error($"Method with name {metadata.Item1} already exists"));
                    return false;
                }
                methods.Add(metadata);
            }
            var status = Nodes.Aggregate(true, (current, cur) => current && cur.CheckNode(compileErrors, variables, methods));
            while (methods.Count != amountOfMethods)
            {
                methods.RemoveAt(methods.Count - 1);
            }
            return status;
        }


        public ClassNode(Token start, Token end) : base(SyntaxType.Class)
        {
            this._start = start;
            this._end = end;
        }
    }
}

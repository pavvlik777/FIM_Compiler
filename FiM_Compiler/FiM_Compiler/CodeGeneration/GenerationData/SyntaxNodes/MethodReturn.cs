using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes
{
    public class MethodReturn : SyntaxNode
    {
        private Token token;
        public override string GenerateCode(string offset = "")
        {
            var output = $"{offset}return {ParseExpression(token.Childs[0])};";
            return output;
        }
        public MethodReturn(Token token) : base(SyntaxType.MethodReturn)
        {
            this.token = token;
        }

        public string GetReturnType(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            return GetExpressionType(token.Childs[0], compileErrors, variables, methods);
        }

        public override bool CheckNode(List<Error> compileErrors, List<(string, string)> variables, List<(string, string)> methods)
        {
            var status = true;
            foreach (var cur in Nodes)
                status = status && cur.CheckNode(compileErrors, variables, methods);
            return status;
        }
    }
}

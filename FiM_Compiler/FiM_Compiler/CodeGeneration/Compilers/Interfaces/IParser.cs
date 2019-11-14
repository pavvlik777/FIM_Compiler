using FiM_Compiler.CodeGeneration.GenerationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.Compilers.Interfaces
{
    public interface IParser
    {
        SyntaxNode GenerateSyntaxTree(List<Token> tokens, List<Error> compileErrors);
        string GenerateCode(SyntaxNode head);
    }
}

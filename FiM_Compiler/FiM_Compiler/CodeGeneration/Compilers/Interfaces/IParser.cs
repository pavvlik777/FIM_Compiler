using FiM_Compiler.CodeGeneration.GenerationData;
using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.Compilers.Interfaces
{
    public interface IParser
    {
        SyntaxNode GenerateSyntaxTree(List<Token> tokens, List<Error> compileErrors);

        string GenerateCode(SyntaxNode head);
    }
}
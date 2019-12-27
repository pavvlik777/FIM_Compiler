using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers.Interfaces
{
    public interface ISyntaxNode
    {
        SyntaxType Type { get; }

        SyntaxNode Node(SyntaxType parent, int start, int end, List<Token> tokens, List<Error> compileErrors);
    }
}
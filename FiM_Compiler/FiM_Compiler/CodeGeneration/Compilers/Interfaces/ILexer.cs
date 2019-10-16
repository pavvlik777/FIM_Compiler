using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers
{
    public interface ILexer
    {
        (List<Token>, bool) PerformLexicalAnalysis(List<Error> compileErrors);
    }
}

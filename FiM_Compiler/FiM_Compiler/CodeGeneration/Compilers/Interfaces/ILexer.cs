using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers.Interfaces
{
    public interface ILexer
    {
        List<Token> PerformLexicalAnalysis(string sourceCode);
    }
}
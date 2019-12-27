using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers.Interfaces
{
    public interface ILexerAnalysis
    {
        List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode);
    }
}
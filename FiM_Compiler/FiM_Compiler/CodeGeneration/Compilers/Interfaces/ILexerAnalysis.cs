using FiM_Compiler.CodeGeneration.GenerationData;
using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.Compilers
{
    public interface ILexerAnalysis
    {
        List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode);
    }
}

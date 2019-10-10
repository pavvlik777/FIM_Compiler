using System;
using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.ErrorsChecks
{
    public interface ILexerErrorCheck
    {
        bool PerformChecks(List<Token> tokens, List<Error> compileErrors);
    }
}

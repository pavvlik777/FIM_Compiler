using System;
using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.ErrorsChecks
{
    public class CommentsChecks : ILexerErrorCheck
    {
        public bool PerformChecks(List<Token> tokens, List<Error> compileErrors)
        {
            foreach (var cur in tokens)
                if (cur.Type == TokenType.CloseMultilineComments)
                {
                    compileErrors.Add(new Error("Unexpected end of multiline comment"));
                    return false;
                }
            return true;
        }
    }
}

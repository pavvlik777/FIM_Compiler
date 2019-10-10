using System;
using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.ErrorsChecks
{
    public class AmountOfEntryPointsCheck : ILexerErrorCheck
    {
        public bool PerformChecks(List<Token> tokens, List<Error> compileErrors)
        {
            int amount = 0;
            foreach (var cur in tokens)
                if (cur.Type == TokenType.MainMethodDeclaration)
                    amount++;
            if(amount < 1)
            {
                compileErrors.Add(new Error("Program must have entry point"));
                return false;
            }
            else if(amount > 1)
            {
                compileErrors.Add(new Error("Program must have only one entry point"));
                return false;
            }
            return true;
        }
    }
}

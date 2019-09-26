using System;
using System.Collections.Generic;

using FiM_Compiler.CodeGeneration.Compilers;

namespace FiM_Compiler.CodeGeneration
{
    public class CompilerFactory
    {
        Dictionary<CompilerMode, Compiler> compilers;

        public CompilerFactory()
        {
            compilers = new Dictionary<CompilerMode, Compiler>()
            {
                { CompilerMode.CSharp, new CSharpCompiler() },
                { CompilerMode.Exe, new ExeCompiler() }
            };
        }

        public void Compile(CompilerMode mode, string sourceCode, string filename)
        {
            compilers[mode].Compile(sourceCode, filename);
            if (compilers[mode].CompileErrors.Count > 0)
            {
                if(compilers[mode].CompileErrors.Count > 1)
                    Console.WriteLine($"Compilation ended with errors");
                else
                    Console.WriteLine($"Compilation ended with error");
                foreach (var cur in compilers[mode].CompileErrors)
                    Console.WriteLine(cur.ToString());
            }
            else
                Console.WriteLine($"Compilation from {filename} completed to {compilers[mode].outputFilename}");
        }
    }
}

using System;
using System.Collections.Generic;

using FiM_Compiler.CodeGeneration.Compilers;

namespace FiM_Compiler.CodeGeneration
{
    public class CompilerFactory
    {
        private readonly Dictionary<CompilerMode, Compiler> _compilers;


        public CompilerFactory()
        {
            _compilers = new Dictionary<CompilerMode, Compiler>()
            {
                { CompilerMode.CSharp, new CSharpCompiler() },
                { CompilerMode.Exe, new ExeCompiler() }
            };
        }


        public void Compile(CompilerMode mode, string sourceCode, string filename)
        {
            _compilers[mode].Compile(sourceCode, filename);
            if (_compilers[mode].CompileErrors.Count > 0)
            {
                Console.WriteLine(_compilers[mode].CompileErrors.Count > 1
                    ? $"Compilation ended with errors"
                    : $"Compilation ended with error");
                foreach (var cur in _compilers[mode].CompileErrors)
                    Console.WriteLine(cur.ToString());
            }
            else
                Console.WriteLine($"Compilation from {filename} completed to {_compilers[mode].OutputFilename}");
        }
    }
}
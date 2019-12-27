using System;
using System.Collections.Generic;
using System.IO;

using FiM_Compiler.CodeGeneration;

namespace FiM_Compiler
{
    public class Program
    {
        private string _sourceName = "SourceCode.fpp";
        private static bool IsDebug = true;

        private string _codeText;


        private static void Main()
        {
            var program = new Program();
            Console.WriteLine("Enter filename with extension");
            program._sourceName = Console.ReadLine();
            var loading = program.LoadFile();
            if (loading)
            {
                var factory = new CompilerFactory();
                factory.Compile(CompilerMode.CSharp, program._codeText, program._sourceName);
            }
            if (!IsDebug)
            {
                return;
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private bool LoadFile()
        {
            var filepath = $@"{Environment.CurrentDirectory}\{_sourceName}";
            var errors = new List<Error>();
            var sourceFile = new FileInfo(filepath);
            if (sourceFile.Extension == ".fpp")
            {
                if (sourceFile.Exists)
                {
                    using (var reader = new StreamReader($"{filepath}", System.Text.Encoding.UTF8))
                    {
                        _codeText = reader.ReadToEnd();
                    }
                }
                else
                {
                    errors.Add(new Error($"File {_sourceName} doesn't exist"));
                }
            }
            else
            {
                errors.Add(new Error("Incorrect file extension"));
            }
            if (errors.Count > 0)
            {
                Console.WriteLine(errors.Count > 1
                    ? $"Compilation ended with errors"
                    : $"Compilation ended with error");
                foreach (var cur in errors)
                {
                    Console.WriteLine(cur.ToString());
                }
            }
            var isOk = errors.Count == 0;

            return isOk;
        }


        #region Constructors
        public Program()
        {
            _codeText = "";
        }
        #endregion
    }
}
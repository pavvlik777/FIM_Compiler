using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using FiM_Compiler.CodeGeneration;

namespace FiM_Compiler
{
    class Program
    {
        string sourceName = "SourceCode.fpp";
        static bool isDebug = true;

        string codeText;

        static void Main()
        {
            Program program = new Program();
            bool loading = program.LoadFile();
            if (loading)
            {
                CompilerFactory factory = new CompilerFactory();
                factory.Compile(CompilerMode.CSharp, program.codeText, program.sourceName);
            }
            if (isDebug)
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        bool LoadFile()
        {
            bool isOK = false;
            string filepath = $@"{Environment.CurrentDirectory}\{sourceName}";
            List<Error> errors = new List<Error>();
            FileInfo sourceFile = new FileInfo(filepath);
            if (sourceFile.Extension.ToUpper(CultureInfo.InvariantCulture) == ".FPP")
            {
                if (sourceFile.Exists)
                {
                    using (StreamReader reader = new StreamReader($"{filepath}", System.Text.Encoding.UTF8))
                    {
                        codeText = reader.ReadToEnd();
                    }
                }
                else
                {
                    errors.Add(new Error($"File {sourceName} doesn't exist"));
                }
            }
            else
            {
                errors.Add(new Error("Incorrect file extension"));
            }
            if (errors.Count > 0)
            {
                if (errors.Count > 1)
                    Console.WriteLine($"Compilation ended with errors");
                else
                    Console.WriteLine($"Compilation ended with error");
                foreach (Error cur in errors)
                {
                    Console.WriteLine(cur.ToString());
                }
            }
            isOK = errors.Count == 0;
            return isOK;
        }

        #region Constructors
        public Program()
        {
            codeText = "";
        }
        #endregion
    }
}

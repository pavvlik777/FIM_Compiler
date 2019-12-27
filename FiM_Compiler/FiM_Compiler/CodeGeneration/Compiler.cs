using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration
{
    public abstract class Compiler
    {
        protected string SourceCode;
        protected string Filename;

        public List<Error> CompileErrors { get; protected set; }
        public string OutputFilename { get; protected set; }


        public Compiler()
        {
            SourceCode = "";
            Filename = "";
        }


        public virtual void Compile(string sourceCode, string filename)
        {
            this.SourceCode = sourceCode;
            this.Filename = filename;
            OutputFilename = "";
        }
    }
}

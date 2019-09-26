using System;
using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration
{
    public abstract class Compiler
    {
        protected List<Error> compileErrors;
        public List<Error> CompileErrors
        {
            get
            {
                return compileErrors;
            }
        }
        public string outputFilename { get; protected set; }
        protected string originalSourceCode;
        protected string sourceCode;
        protected string filename;

        public Compiler()
        {
            sourceCode = "";
            filename = "";
        }

        public virtual void Compile(string sourceCode, string filename)
        {
            this.originalSourceCode = sourceCode;
            this.sourceCode = sourceCode;
            this.filename = filename;
            outputFilename = "";
        }
    }
}

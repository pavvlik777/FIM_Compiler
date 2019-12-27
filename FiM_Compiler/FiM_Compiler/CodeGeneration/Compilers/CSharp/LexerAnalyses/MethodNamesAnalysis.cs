using System.Collections.Generic;
using System.Linq;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalyses
{
    public class MethodNamesAnalysis : ILexerAnalysis
    {
        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            var currentIndex = 0;
            var start = 0;
            var methods = new List<string>();
            var isClass = false;
            while(currentIndex < tokens.Count)
            {
                if(isClass)
                {
                    switch(tokens[currentIndex].Type)
                    {
                        case TokenType.ClassDeclaration:
                            UpdateMethodNames(start, currentIndex, tokens, methods);
                            start = currentIndex;
                            methods = new List<string>();
                            break;
                        case TokenType.InterfaceDeclaration:
                            isClass = false;
                            UpdateMethodNames(start, currentIndex, tokens, methods);
                            break;
                        case TokenType.MainMethodDeclaration:
                        case TokenType.MethodDeclaration:
                            methods.Add(tokens[currentIndex].Childs[0].Value);
                            break;
                    }
                }
                else
                {
                    switch (tokens[currentIndex].Type)
                    {
                        case TokenType.ClassDeclaration:
                            isClass = true;
                            start = currentIndex;
                            methods = new List<string>();
                            break;
                    }
                }
                currentIndex++;
            }
            UpdateMethodNames(start, tokens.Count, tokens, methods);
            return tokens;
        }

        void UpdateMethodNames(int start, int end, List<Token> tokens, List<string> methods)
        {
            for(var i = start; i < end; i++)
            {
                if(tokens[i].Type == TokenType.Name && methods.Any(x => x == tokens[i].Value))
                {
                    tokens[i].Type = TokenType.MethodName;
                }
            }
        }

        #region Constructor
        public MethodNamesAnalysis()
        {

        }
        #endregion
    }
}

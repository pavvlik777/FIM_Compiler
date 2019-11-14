using FiM_Compiler.CodeGeneration.GenerationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalysises
{
    public class MethodNamesAnalysis : ILexerAnalysis
    {
        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            int currentIndex = 0;
            int start = 0;
            List<string> methods = new List<string>();
            bool isClass = false;
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
            for(int i = start; i < end; i++)
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

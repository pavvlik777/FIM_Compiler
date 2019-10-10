using System;
using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.ErrorsChecks
{
    public class ChecksBounds : ILexerErrorCheck
    {
        private enum LevelType
        {
            Program, EOF, Class, Interface, Method
        }

        public bool PerformChecks(List<Token> tokens, List<Error> compileErrors)
        {
            int currentIndex = 0;
            int endIndex = tokens.Count;
            bool isEOF = false;
            bool status = true;
            do
            {
                (bool, LevelType) output = Check(tokens, LevelType.Program, compileErrors, ref currentIndex, ref endIndex);
                status = output.Item1;
                if (!status)
                    break;
                isEOF = output.Item2 == LevelType.EOF;
            } while (!isEOF);
            return status;
        }

        (bool, LevelType) Check(List<Token> tokens, LevelType parent, List<Error> compileErrors, ref int currentIndex, ref int endIndex)
        {
            if (currentIndex >= endIndex)
                return (true, LevelType.EOF);   
            switch(tokens[currentIndex].Type)
            {
                case TokenType.ClassDeclaration:
                    return ClassCheck(tokens, parent, compileErrors, ref currentIndex, ref endIndex);
                case TokenType.InterfaceDeclaration:
                    return InterfaceCheck(tokens, parent, compileErrors, ref currentIndex, ref endIndex);
                case TokenType.ClassEndDeclaration:
                    compileErrors.Add(new Error("Unexpected class end declaration"));
                    return (false, LevelType.EOF);
                case TokenType.MethodEndDeclaration:
                    compileErrors.Add(new Error("Unexpected method end declaration"));
                    return (false, LevelType.EOF);
                case TokenType.MainMethodDeclaration:
                case TokenType.MethodDeclaration:
                    return MethodCheck(tokens, parent, compileErrors, ref currentIndex, ref endIndex);
                default:
                    currentIndex++;
                    return (true, parent);
            }
        }

        #region Checks
        (bool, LevelType) MethodCheck(List<Token> tokens, LevelType parent, List<Error> compileErrors, ref int currentIndex, ref int endIndex)
        {
            if (parent == LevelType.Method)
            {
                compileErrors.Add(new Error("Unexpected method declaration"));
                return (false, LevelType.EOF);
            }
            if(parent == LevelType.Interface)
            {
                if(tokens[currentIndex].Type == TokenType.MainMethodDeclaration)
                {
                    compileErrors.Add(new Error("Unexpected entry point declaration"));
                    return (false, LevelType.EOF);
                }
                currentIndex++;
                return (true, LevelType.Method);
            }
            int i = currentIndex;
            int starts = 1;
            int ends = 0;
            while (starts != ends)
            {
                i++;
                if (i < endIndex)
                {
                    if (tokens[i].Type == TokenType.MethodDeclaration || tokens[i].Type == TokenType.MainMethodDeclaration) starts++;
                    else if (tokens[i].Type == TokenType.MethodEndDeclaration) ends++;
                }
                else
                {
                    compileErrors.Add(new Error("Incorrect method declaration"));
                    return (false, LevelType.EOF);
                }
            }
            bool isEOF = false;
            starts = currentIndex + 1;
            ends = i - 1;
            do
            {
                (bool, LevelType) output = Check(tokens, LevelType.Method, compileErrors, ref starts, ref ends);
                if (!output.Item1)
                {
                    return (false, LevelType.EOF);
                }
                isEOF = output.Item2 == LevelType.EOF;
            } while (!isEOF);
            currentIndex = i + 1;
            return (true, LevelType.Method);
        }

        (bool, LevelType) ClassCheck(List<Token> tokens, LevelType parent, List<Error> compileErrors, ref int currentIndex, ref int endIndex)
        {
            if(parent == LevelType.Method)
            {
                compileErrors.Add(new Error("Unexpected class declaration"));
                return (false, LevelType.EOF);
            }
            int i = currentIndex;
            int starts = 1;
            int ends = 0;
            while(starts != ends)
            {
                i++;
                if (i < endIndex)
                {
                    if (tokens[i].Type == TokenType.ClassDeclaration || tokens[i].Type == TokenType.InterfaceDeclaration) starts++;
                    else if (tokens[i].Type == TokenType.ClassEndDeclaration) ends++;
                }
                else
                {
                    compileErrors.Add(new Error("Incorrect class declaration"));
                    return (false, LevelType.EOF);
                }
            }
            bool isEOF = false;
            starts = currentIndex + 1;
            ends = i - 1;
            do
            {
                (bool, LevelType) output = Check(tokens, LevelType.Class, compileErrors, ref starts, ref ends);
                if(!output.Item1)
                {
                    return (false, LevelType.EOF);
                }
                isEOF = output.Item2 == LevelType.EOF;
            } while (!isEOF);
            currentIndex = i + 1;
            return (true, LevelType.Class);
        }

        (bool, LevelType) InterfaceCheck(List<Token> tokens, LevelType parent, List<Error> compileErrors, ref int currentIndex, ref int endIndex)
        {
            if (parent != LevelType.Program)
            {
                compileErrors.Add(new Error("Unexpected interface declaration"));
                return (false, LevelType.EOF);
            }
            int i = currentIndex;
            int starts = 1;
            int ends = 0;
            while (starts != ends)
            {
                i++;
                if (i < endIndex)
                {
                    if (tokens[i].Type == TokenType.ClassDeclaration || tokens[i].Type == TokenType.InterfaceDeclaration) starts++;
                    else if (tokens[i].Type == TokenType.ClassEndDeclaration) ends++;
                }
                else
                {
                    compileErrors.Add(new Error("Incorrect interface declaration"));
                    return (false, LevelType.EOF);
                }
            }
            bool isEOF = false;
            starts = currentIndex + 1;
            ends = i - 1;
            do
            {
                (bool, LevelType) output = Check(tokens, LevelType.Interface, compileErrors, ref starts, ref ends);
                if (!output.Item1)
                {
                    return (false, LevelType.EOF);
                }
                isEOF = output.Item2 == LevelType.EOF;
            } while (!isEOF);
            currentIndex = i + 1;
            return (true, LevelType.Interface);
        }
        #endregion
    }
}
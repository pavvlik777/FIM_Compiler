using FiM_Compiler.CodeGeneration.GenerationData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalysises
{
    public class VariablesNamesAnalysis : ILexerAnalysis //TODO test
    {
        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            int currentIndex = 0;
            int start = 0;
            List<string> variables = new List<string>();
            Stack<TokenType> innerAreas = new Stack<TokenType>();
            Stack<string> tempVariables = new Stack<string>();
            bool isClass = false;
            while (currentIndex < tokens.Count)
            {
                if (isClass)
                {
                    switch (tokens[currentIndex].Type)
                    {
                        case TokenType.ClassDeclaration:
                            UpdateMethodNames(start, currentIndex, tokens, variables, tempVariables);
                            start = currentIndex;
                            variables = new List<string>();
                            tempVariables = new Stack<string>();
                            break;
                        case TokenType.InterfaceDeclaration:
                            isClass = false;
                            UpdateMethodNames(start, currentIndex, tokens, variables, tempVariables);
                            break;
                        case TokenType.VariableDeclarationAndAssign:
                        case TokenType.VariableDeclaration:
                            variables.Add(tokens[currentIndex].Childs[0].Value);
                            break;
                        case TokenType.ForStartWithDeclaring:
                        case TokenType.ForeachStartWithDeclaring:
                            innerAreas.Push(tokens[currentIndex].Type);
                            tempVariables.Push(tokens[currentIndex].Childs[1].Value);
                            break;
                        case TokenType.WhileStart:
                        case TokenType.ForStart:
                        case TokenType.ForeachStart:
                        case TokenType.SwitchDeclaration:
                            innerAreas.Push(tokens[currentIndex].Type);
                            break;
                        case TokenType.CycleEnding:
                            if(innerAreas.Count != 0)
                            {
                                TokenType area = innerAreas.Pop();
                                if(area == TokenType.ForStartWithDeclaring || area == TokenType.ForeachStartWithDeclaring)
                                    tempVariables.Pop();
                            }
                            break;
                        case TokenType.MainMethodDeclaration:
                        case TokenType.MethodDeclaration:
                            UpdateMethodNames(start, currentIndex, tokens, variables, tempVariables);
                            start = currentIndex;
                            variables = new List<string>();
                            tempVariables = new Stack<string>();
                            for (int j = 3; j < tokens[currentIndex].Childs.Count; j += 2)
                                variables.Add(tokens[currentIndex].Childs[j].Value);
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
                            variables = new List<string>();
                            break;
                    }
                }
                currentIndex++;
            }
            UpdateMethodNames(start, tokens.Count, tokens, variables, tempVariables);
            return tokens;
        }

        void UpdateMethodNames(int start, int end, List<Token> tokens, List<string> variables, Stack<string> stack)
        {
            for (int i = start; i < end; i++)
            {
                if (tokens[i].Type == TokenType.Name && (variables.Any(x => x == tokens[i].Value) || stack.Any(x => x == tokens[i].Value)))
                {
                    tokens[i].Type = TokenType.VariableName;
                }
            }
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.ErrorsChecks
{
    public class ExtendedNamesErrorChecks : ILexerErrorCheck
    {
        private enum LevelType
        {
            Program, EOF, Class, Interface, Method, Cycle
        }

        List<string> classes;
        List<string> interfaces;
        List<string> methods;
        List<string> variables;

        public bool PerformChecks(List<Token> tokens, List<Error> compileErrors)
        {
            int currentIndex = 0;
            int endIndex = tokens.Count;
            bool isEOF = false;
            bool status = true;
            classes = new List<string>();
            interfaces = new List<string>();
            methods = new List<string>();
            variables = new List<string>();
            do
            {
                (bool, LevelType) output = Check(tokens, LevelType.Program, compileErrors, ref currentIndex, ref endIndex, classes, interfaces, methods, variables);
                status = output.Item1;
                if (!status)
                    break;
                isEOF = output.Item2 == LevelType.EOF;
            } while (!isEOF);
            return status;
        }

        (bool, LevelType) Check(List<Token> tokens, LevelType parent, List<Error> compileErrors, ref int currentIndex, ref int endIndex, 
            List<string> classes, List<string> interfaces, List<string> methods, List<string> variables)
        {
            if (currentIndex >= endIndex)
                return (true, LevelType.EOF);
            List<string> _classes = new List<string>(classes);
            List<string> _interfaces = new List<string>(interfaces);
            List<string> _methods = new List<string>(methods);
            List<string> _variables = new List<string>(variables);
            if(parent == LevelType.Cycle)
            {
                if (tokens[currentIndex - 1].Childs.Count > 2)
                {
                    _variables.Add(tokens[currentIndex - 1].Childs[2].Value);
                    for (int j = currentIndex - 1; j < endIndex; j++)
                    {
                        tokens[j].Type = UpdateVariblesTokenTypes(tokens[j], _variables);
                        if (tokens[j].Childs == null) continue;
                        foreach (var child in tokens[j].Childs)
                            child.Type = UpdateVariblesTokenTypes(child, _variables);
                    }
                }
            }
            if(parent == LevelType.Method)
            {
                if (tokens[currentIndex - 1].Childs.Count > 2)
                {
                    for (int j = 3; j < tokens[currentIndex - 1].Childs.Count; j += 2)
                        _variables.Add(tokens[currentIndex - 1].Childs[j].Value);
                }
            }
            for (int i = currentIndex; i < endIndex; i++)
            {
                switch(tokens[i].Type)
                {
                    case TokenType.ForStartWithDeclaring:
                        BoundSkip(tokens, new TokenType[] { TokenType.ForStartWithDeclaring, TokenType.ForeachStartWithDeclaring, TokenType.SwitchDeclaration },
                            new TokenType[] { TokenType.CycleEnding }, ref i, ref endIndex);
                        break;
                    case TokenType.ForeachStartWithDeclaring:
                        BoundSkip(tokens, new TokenType[] { TokenType.ForStartWithDeclaring, TokenType.ForeachStartWithDeclaring, TokenType.SwitchDeclaration },
                            new TokenType[] { TokenType.CycleEnding }, ref i, ref endIndex);
                        break;
                    case TokenType.ClassDeclaration:
                        _classes.Add(tokens[i].Childs[1].Value);
                        BoundSkip(tokens, new TokenType[] { TokenType.ClassDeclaration, TokenType.InterfaceDeclaration }, new TokenType[] { TokenType.ClassEndDeclaration }, ref i, ref endIndex);
                        break;
                    case TokenType.InterfaceDeclaration:
                        _interfaces.Add(tokens[i].Childs[0].Value);
                        BoundSkip(tokens, new TokenType[] { TokenType.ClassDeclaration, TokenType.InterfaceDeclaration }, new TokenType[] { TokenType.ClassEndDeclaration }, ref i, ref endIndex);
                        break;
                    case TokenType.MainMethodDeclaration:
                    case TokenType.MethodDeclaration:
                        _methods.Add(tokens[i].Childs[0].Value);
                        if (parent == LevelType.Interface)
                            i++;
                        else
                            BoundSkip(tokens, new TokenType[] { TokenType.MainMethodDeclaration, TokenType.MethodDeclaration }, new TokenType[] { TokenType.MethodEndDeclaration }, ref i, ref endIndex);
                        break;
                    case TokenType.VariableDeclaration:
                    case TokenType.VariableDeclarationAndAssign:
                        _variables.Add(tokens[i].Childs[0].Value);
                        for (int j = i; j < endIndex; j++)
                        {
                            tokens[j].Type = UpdateVariblesTokenTypes(tokens[j], _variables);
                            if (tokens[j].Childs == null) continue;
                            foreach (var child in tokens[j].Childs)
                                child.Type = UpdateVariblesTokenTypes(child, _variables);
                        }
                        BoundSkip(tokens, null, null, ref i, ref endIndex);
                        break;
                }
            }
            if (!CheckNames(_classes, _interfaces, _methods, _variables, compileErrors))
                return (false, LevelType.EOF);
            (bool, LevelType) result = (true, LevelType.EOF);
            int start = 1;
            int end = 0;
            for(int i = currentIndex; i < endIndex; i++)
            {
                switch (tokens[i].Type)
                {
                    case TokenType.ForStartWithDeclaring:
                        start = i + 1;
                        end = BoundSkip(tokens, new TokenType[] { TokenType.ForStartWithDeclaring, TokenType.ForeachStartWithDeclaring, TokenType.SwitchDeclaration },
                            new TokenType[] { TokenType.CycleEnding }, ref i, ref endIndex);
                        result = Check(tokens, LevelType.Cycle, compileErrors, ref start, ref end, _classes, _interfaces, _methods, _variables);
                        if (!result.Item1)
                            return result;
                        break;
                    case TokenType.ForeachStartWithDeclaring:
                        start = i + 1;
                        end = BoundSkip(tokens, new TokenType[] { TokenType.ForStartWithDeclaring, TokenType.ForeachStartWithDeclaring, TokenType.SwitchDeclaration },
                            new TokenType[] { TokenType.CycleEnding }, ref i, ref endIndex);
                        result = Check(tokens, LevelType.Cycle, compileErrors, ref start, ref end, _classes, _interfaces, _methods, _variables);
                        if (!result.Item1)
                            return result;
                        break;
                    case TokenType.ClassDeclaration:
                        start = i + 1;
                        end = BoundSkip(tokens, new TokenType[] { TokenType.ClassDeclaration, TokenType.InterfaceDeclaration }, new TokenType[] { TokenType.ClassEndDeclaration }, ref i, ref endIndex);
                        result = Check(tokens, LevelType.Class, compileErrors, ref start, ref end, _classes, _interfaces, _methods, _variables);
                        if (!result.Item1)
                            return result;
                        break;
                    case TokenType.InterfaceDeclaration:
                        start = i + 1;
                        end = BoundSkip(tokens, new TokenType[] { TokenType.ClassDeclaration, TokenType.InterfaceDeclaration }, new TokenType[] { TokenType.ClassEndDeclaration }, ref i, ref endIndex);
                        result = Check(tokens, LevelType.Interface, compileErrors, ref start, ref end, _classes, _interfaces, _methods, _variables);
                        if (!result.Item1)
                            return result;
                        break;
                    case TokenType.MethodDeclaration:
                    case TokenType.MainMethodDeclaration:
                        if(parent == LevelType.Interface)
                        {
                            i++;
                            break;
                        }
                        start = i + 1;
                        end = BoundSkip(tokens, new TokenType[] { TokenType.MainMethodDeclaration, TokenType.MethodDeclaration }, new TokenType[] { TokenType.MethodEndDeclaration }, ref i, ref endIndex);
                        result = Check(tokens, LevelType.Method, compileErrors, ref start, ref end, _classes, _interfaces, _methods, _variables);
                        if (!result.Item1)
                            return result;
                        break;
                }
            }

            for(int i = currentIndex; i < endIndex; i++)
            {
                tokens[i].Type = UpdateTokenTypes(tokens[i], _classes, _interfaces, _methods, _variables);
                if (tokens[i].Childs == null) continue;
                foreach(var child in tokens[i].Childs)
                    child.Type = UpdateTokenTypes(child, _classes, _interfaces, _methods, _variables);
            }
            if (parent == LevelType.Method)
            {
                if (tokens[currentIndex - 1].Childs.Count > 2)
                {
                    for (int j = 3; j < tokens[currentIndex - 1].Childs.Count; j += 2)
                        tokens[currentIndex - 1].Childs[j].Type = TokenType.MethodVariableName;
                }
            }
            if(parent == LevelType.Class)
            {
                if (tokens[currentIndex - 1].Childs.Count > 2)
                {
                    for (int j = 2; j < tokens[currentIndex - 1].Childs.Count; j++)
                        tokens[currentIndex - 1].Childs[j].Type = TokenType.ClassExtendsName;
                }
            }
            if (parent == LevelType.Interface)
            {
                if (tokens[currentIndex - 1].Childs.Count > 1)
                {
                    for (int j = 1; j < tokens[currentIndex - 1].Childs.Count; j++)
                        tokens[currentIndex - 1].Childs[j].Type = TokenType.ClassExtendsName;
                }
            }
            return (true, LevelType.EOF); ;
        }

        int BoundSkip(List<Token> tokens, TokenType[] start, TokenType[] end, ref int currentIndex, ref int endIndex)
        {
            if(start == null && end == null)
            {
                currentIndex++;
                return currentIndex - 1;
            }
            int i = currentIndex;
            int starts = 1;
            int ends = 0;
            while (starts != ends)
            {
                i++;
                if (i < endIndex)
                {
                    if (IsIn(tokens[i].Type, start)) starts++;
                    else if (IsIn(tokens[i].Type, end)) ends++;
                }
            }
            currentIndex = i + 1;
            return i - 1;
        }

        bool IsIn(TokenType type, TokenType[] checks)
        {
            foreach (var cur in checks)
                if (type == cur) return true;
            return false;
        }

        bool CheckNames(List<string> classes, List<string> interfaces, List<string> methods, List<string> variables, List<Error> compileErrors)
        {
            bool status = true;
            status = CheckSuperclass(classes, interfaces, methods, variables, compileErrors);
            if(!status) return false;
            status = CheckUnic(classes, interfaces, methods, variables, compileErrors);
            if (!status) return false;
            status = CheckIntersections(classes, interfaces, methods, variables, compileErrors);
            if (!status) return false;
            return true;
        }
        TokenType UpdateVariblesTokenTypes(Token token, List<string> variables)
        {
            if (token.Type == TokenType.Name)
            {
                if (variables.Any(x => x == token.Value)) return TokenType.VariableName;
            }
            return token.Type;
        }

        TokenType UpdateTokenTypes(Token token, List<string> classes, List<string> interfaces, List<string> methods, List<string> variables)
        {
            if (token.Type == TokenType.Name)
            {
                if (token.Value == "Princess Celestia") return TokenType.MainSuperclassName;
                if (token.Value == "nothing") return TokenType.NullLiteral;
                if (token.Value == "yes" || token.Value == "true" || token.Value == "right" || token.Value == "correct" ||
                    token.Value == "no" || token.Value == "false" || token.Value == "wrong" || token.Value == "incorrect") return TokenType.BoolLiteral;
                if (int.TryParse(token.Value, out _)) return TokenType.IntLiteral;
                if (token.Value.Length == 3) if (token.Value[0] == '\'' && token.Value[2] == '\'') return TokenType.CharLiteral;
                if (token.Value.Length >= 2) if (token.Value[0] == '"' && token.Value[token.Value.Length - 1] == '"') return TokenType.StringLiteral;
                if (classes.Any(x => x == token.Value)) return TokenType.ClassName;
                if (interfaces.Any(x => x == token.Value)) return TokenType.InterfaceName;
                if (methods.Any(x => x == token.Value)) return TokenType.MethodName;
                //if (variables.Any(x => x == token.Value)) return TokenType.VariableName;
            }
            else
            {
                //TODO mb other cases
            }
            return token.Type;
        }

        #region Checks
        bool CheckIntersections(List<string> classes, List<string> interfaces, List<string> methods, List<string> variables, List<Error> compileErrors)
        {
            (bool, string) temp = IsListsHaveCommon(classes, methods);
            if (temp.Item1)
            {
                compileErrors.Add(new Error($"Method and class {temp.Item2} must have different names"));
                return false;
            }
            temp = IsListsHaveCommon(classes, variables);
            if (temp.Item1)
            {
                compileErrors.Add(new Error($"Variable and class {temp.Item2} must have different names"));
                return false;
            }
            temp = IsListsHaveCommon(classes, interfaces);
            if (temp.Item1)
            {
                compileErrors.Add(new Error($"Class and interface {temp.Item2} must have different names"));
                return false;
            }
            temp = IsListsHaveCommon(interfaces, methods);
            if (temp.Item1)
            {
                compileErrors.Add(new Error($"Interface and method {temp.Item2} must have different names"));
                return false;
            }
            temp = IsListsHaveCommon(interfaces, variables);
            if (temp.Item1)
            {
                compileErrors.Add(new Error($"Interface and variable {temp.Item2} must have different names"));
                return false;
            }
            temp = IsListsHaveCommon(methods, variables);
            if (temp.Item1)
            {
                compileErrors.Add(new Error($"Method and variable {temp.Item2} must have different names"));
                return false;
            }
            return true;
        }

        bool CheckUnic(List<string> classes, List<string> interfaces, List<string> methods, List<string> variables, List<Error> compileErrors)
        {
            (bool, string) temp = IsAllItemsUnic(classes);
            if (!temp.Item1)
            {
                compileErrors.Add(new Error($"Classes {temp.Item2} must have different names"));
                return false;
            }
            temp = IsAllItemsUnic(interfaces);
            if (!temp.Item1)
            {
                compileErrors.Add(new Error($"Interfaces {temp.Item2} must have different names"));
                return false;
            }
            temp = IsAllItemsUnic(methods);
            if (!temp.Item1)
            {
                compileErrors.Add(new Error($"Methods {temp.Item2} must have different names"));
                return false;
            }
            temp = IsAllItemsUnic(variables);
            if (!temp.Item1)
            {
                compileErrors.Add(new Error($"Variables {temp.Item2} must have different names"));
                return false;
            }
            return true;
        }

        bool CheckSuperclass(List<string> classes, List<string> interfaces, List<string> methods, List<string> variables, List<Error> compileErrors)
        {
            bool status = true;
            status = classes.Any(x => x == "Princess Celestia");
            if (status)
            {
                compileErrors.Add(new Error("Class with name Princess Celestia can't be declared"));
                return false;
            }
            status = interfaces.Any(x => x == "Princess Celestia");
            if (status)
            {
                compileErrors.Add(new Error("Interface with name Princess Celestia can't be declared"));
                return false;
            }
            status = methods.Any(x => x == "Princess Celestia");
            if (status)
            {
                compileErrors.Add(new Error("Method with name Princess Celestia can't be declared"));
                return false;
            }
            status = variables.Any(x => x == "Princess Celestia");
            if (status)
            {
                compileErrors.Add(new Error("Variable with name Princess Celestia can't be declared"));
                return false;
            }
            return true;
        }

        (bool, T) IsListsHaveCommon<T>(IEnumerable<T> first, IEnumerable<T> second) where T: class, IComparable<T>
        {
            List<T> test = first.Intersect(second).ToList();
            if (test.Count > 0)
                return (true, test[0]);
            return (false, null);
        }

        (bool, T) IsAllItemsUnic<T>(List<T> input) where T: class, IComparable<T>
        {
            List<T> work = new List<T>(input);
            work.Sort();
            for(int i = 0; i < work.Count - 1; i++)
            {
                if(work[i].CompareTo(work[i + 1]) == 0)
                    return (false, work[i]);
            }
            return (true, null);
        }
        #endregion
    }
}

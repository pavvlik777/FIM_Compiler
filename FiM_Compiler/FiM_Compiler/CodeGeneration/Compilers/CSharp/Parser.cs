using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp
{
    public class Parser : IParser
    {
        public string GenerateCode(SyntaxNode head)
        {
            return head.GenerateCode();
        }

        public bool PerformTypesCheck(SyntaxNode head, List<Error> compileErrors)
        {
            return head.CheckNode(compileErrors, new List<(string, string)>(), new List<(string, string)>());
        }

        public SyntaxNode GenerateSyntaxTree(List<Token> tokens, List<Error> compileErrors)
        {
            if(tokens.Count == 0)
            {
                compileErrors.Add(new Error("Empty program"));
                return null;
            }
            SyntaxNode output = Node(SyntaxType.Program, 0, tokens.Count, tokens, compileErrors);
            return output;
        }

        SyntaxNode Node(SyntaxType parent, int start, int end, List<Token> tokens, List<Error> compileErrors)
        {
            if(parent == SyntaxType.Program)
            {
                SyntaxNode node = new ProgramNode();
                for(int i = start; i < end; i++)
                {
                    if(tokens[i].Type == TokenType.ClassDeclaration)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while(left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of class declaration"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.ClassDeclaration || tokens[i].Type == TokenType.InterfaceDeclaration) left++;
                            else if (tokens[i].Type == TokenType.ClassEndDeclaration) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.Class, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.InterfaceDeclaration)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of interface declaration"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.ClassDeclaration || tokens[i].Type == TokenType.InterfaceDeclaration) left++;
                            else if (tokens[i].Type == TokenType.ClassEndDeclaration) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.Interface, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if(IsTokenWhiteSpace(tokens[i]))
                    {
                        continue;
                    }
                    else
                    {
                        compileErrors.Add(new Error("Unexpected expression in program body"));
                        return null;
                    }
                }
                return node;
            }
            else if(parent == SyntaxType.Class)
            {
                SyntaxNode node = new ClassNode(tokens[start - 1], tokens[end]);
                for (int i = start; i < end; i++)
                {
                    if(tokens[i].Type == TokenType.MethodDeclaration)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of method declaration"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.MethodDeclaration || tokens[i].Type == TokenType.MainMethodDeclaration) left++;
                            else if (tokens[i].Type == TokenType.MethodEndDeclaration) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.MethodDeclaring, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.MainMethodDeclaration)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of main method declaration"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.MethodDeclaration || tokens[i].Type == TokenType.MainMethodDeclaration) left++;
                            else if (tokens[i].Type == TokenType.MethodEndDeclaration) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.MainMethodDeclaring, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (IsTokenWhiteSpace(tokens[i]))
                    {
                        continue;
                    }
                    else
                    {
                        compileErrors.Add(new Error("Unexpected expression in class body"));
                        return null;
                    }
                }
                return node;
            }
            else if (parent == SyntaxType.Interface)
            {
                SyntaxNode node = new InterfaceNode(tokens[start - 1], tokens[end]);
                for (int i = start; i < end; i++)
                {
                    if (tokens[i].Type == TokenType.MethodDeclaration)
                    {
                        node.Nodes.Add(new InterfaceMethodNode(tokens[i]));
                    }
                    else if (IsTokenWhiteSpace(tokens[i]))
                    {
                        continue;
                    }
                    else
                    {
                        compileErrors.Add(new Error("Unexpected expression in interface body"));
                        return null;
                    }
                }
                return node;
            }
            else if(parent == SyntaxType.MethodDeclaring || parent == SyntaxType.MainMethodDeclaring)
            {
                SyntaxNode node = new MethodNode(tokens[start - 1], tokens[end], parent == SyntaxType.MainMethodDeclaring);
                Node(parent, start, end, tokens, compileErrors, node, "method");
                return node;
            }
            else if (parent == SyntaxType.Switch)
            {
                SyntaxNode node = new SwitchNode(tokens[start - 1], tokens[end]);
                Node(parent, start, end, tokens, compileErrors, node, "switch");
                return node;
            }
            else if (parent == SyntaxType.SwitchCase)
            {
                SyntaxNode node = new SwitchCase(tokens[start - 1], tokens[start - 1].Type == TokenType.SwitchDefaultCase);
                Node(parent, start, end, tokens, compileErrors, node, "switch case");
                return node;
            }
            else if (parent == SyntaxType.IfTruePart)
            {
                SyntaxNode node = new IfTrueNode(tokens[start - 1].Childs[0]);
                Node(parent, start, end, tokens, compileErrors, node, "if");
                return node;
            }
            else if (parent == SyntaxType.IfFalsePart)
            {
                SyntaxNode node = new IfElseNode();
                Node(parent, start, end, tokens, compileErrors, node, "else");
                return node;
            }
            else if (parent == SyntaxType.ElifPart)
            {
                SyntaxNode node = new ElifNode(tokens[start - 1].Childs[0]);
                Node(parent, start, end, tokens, compileErrors, node, "else-if");
                return node;
            }
            else if (parent == SyntaxType.For)
            {
                SyntaxNode node = new ForNode(tokens[start - 1], tokens[end]);
                Node(parent, start, end, tokens, compileErrors, node, "for");
                return node;
            }
            else if (parent == SyntaxType.Foreach)
            {
                SyntaxNode node = new ForeachNode(tokens[start - 1], tokens[end]);
                Node(parent, start, end, tokens, compileErrors, node, "foreach");
                return node;
            }
            else if (parent == SyntaxType.While)
            {
                SyntaxNode node = new WhileNode(tokens[start - 1].Childs[0]);
                Node(parent, start, end, tokens, compileErrors, node, "while");
                return node;
            }
            else if (parent == SyntaxType.DoWhile)
            {
                SyntaxNode node = new DoWhileNode(tokens[end].Childs[0]);
                Node(parent, start, end, tokens, compileErrors, node, "do-while");
                return node;
            }

            compileErrors.Add(new Error("Not implemented parser behaviour"));
            return null;
        }
        SyntaxNode Node(SyntaxType parent, int start, int end, List<Token> tokens, List<Error> compileErrors, SyntaxNode node, string errorMsg)
        {
            for (int i = start; i < end; i++)
            {
                if (parent == SyntaxType.Switch)
                {
                    if (tokens[i].Type == TokenType.SwitchCase)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of switch case statement"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.SwitchCase) left++;
                            else if (tokens[i].Type == TokenType.SwitchCaseEnd) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.SwitchCase, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.SwitchDefaultCase)
                    {
                        int newStart = i + 1;
                        i = end - 1;
                        SyntaxNode childNode = Node(SyntaxType.SwitchCase, newStart, end, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (IsTokenWhiteSpace(tokens[i]) || tokens[i].Type == TokenType.SwitchCaseEnd)
                    {
                        continue;
                    }
                    else
                    {
                        compileErrors.Add(new Error($"Unexpected expression in {errorMsg} body - {tokens[i].Value}"));
                        return null;
                    }
                }
                else
                {
                    if (tokens[i].Type == TokenType.IfStart)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of if statement"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.IfStart || tokens[i].Type == TokenType.IfElse || tokens[i].Type == TokenType.Elif) left++;
                            else if (tokens[i].Type == TokenType.IfEnd) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.IfTruePart, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.Elif)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of else part of if statement"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.IfStart || tokens[i].Type == TokenType.IfElse || tokens[i].Type == TokenType.Elif) left++;
                            else if (tokens[i].Type == TokenType.IfEnd) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.ElifPart, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.IfElse)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of else part of if statement"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.IfStart || tokens[i].Type == TokenType.IfElse || tokens[i].Type == TokenType.Elif) left++;
                            else if (tokens[i].Type == TokenType.IfEnd) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.IfFalsePart, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.ForStart || tokens[i].Type == TokenType.ForStartWithDeclaring)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of for statement"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.ForStart || tokens[i].Type == TokenType.ForStartWithDeclaring ||
                                tokens[i].Type == TokenType.ForeachStart || tokens[i].Type == TokenType.ForeachStartWithDeclaring ||
                                tokens[i].Type == TokenType.SwitchDeclaration || tokens[i].Type == TokenType.WhileStart) left++;
                            else if (tokens[i].Type == TokenType.CycleEnding) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.For, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.ForeachStart || tokens[i].Type == TokenType.ForeachStartWithDeclaring)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of foreach statement"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.ForStart || tokens[i].Type == TokenType.ForStartWithDeclaring ||
                                tokens[i].Type == TokenType.ForeachStart || tokens[i].Type == TokenType.ForeachStartWithDeclaring ||
                                tokens[i].Type == TokenType.SwitchDeclaration || tokens[i].Type == TokenType.WhileStart) left++;
                            else if (tokens[i].Type == TokenType.CycleEnding) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.Foreach, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.WhileStart)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of while statement"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.ForStart || tokens[i].Type == TokenType.ForStartWithDeclaring ||
                                tokens[i].Type == TokenType.ForeachStart || tokens[i].Type == TokenType.ForeachStartWithDeclaring ||
                                tokens[i].Type == TokenType.SwitchDeclaration || tokens[i].Type == TokenType.WhileStart) left++;
                            else if (tokens[i].Type == TokenType.CycleEnding) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.While, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.DoWhileStart)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of do while statement"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.DoWhileStart) left++;
                            else if (tokens[i].Type == TokenType.DoWhileEnd) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.DoWhile, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.SwitchDeclaration)
                    {
                        int newStart = i + 1;
                        int left = 1;
                        int right = 0;
                        while (left != right)
                        {
                            i++;
                            if (i >= end)
                            {
                                compileErrors.Add(new Error("Missing end of switch statement"));
                                return null;
                            }
                            if (tokens[i].Type == TokenType.ForStart || tokens[i].Type == TokenType.ForStartWithDeclaring ||
                                tokens[i].Type == TokenType.ForeachStart || tokens[i].Type == TokenType.ForeachStartWithDeclaring ||
                                tokens[i].Type == TokenType.SwitchDeclaration || tokens[i].Type == TokenType.WhileStart) left++;
                            else if (tokens[i].Type == TokenType.CycleEnding) right++;
                        }
                        SyntaxNode childNode = Node(SyntaxType.Switch, newStart, i, tokens, compileErrors);
                        if (childNode == null)
                            return null;
                        node.Nodes.Add(childNode);
                    }
                    else if (tokens[i].Type == TokenType.MethodCalling)
                    {
                        node.Nodes.Add(new MethodCalling(tokens[i]));
                    }
                    else if (tokens[i].Type == TokenType.MethodReturn)
                    {
                        node.Nodes.Add(new MethodReturn(tokens[i]));
                    }
                    else if (tokens[i].Type == TokenType.VariableDeclaration)
                    {
                        node.Nodes.Add(new VariableDeclarationNode(tokens[i]));
                    }
                    else if (tokens[i].Type == TokenType.VariableDeclarationAndAssign)
                    {
                        node.Nodes.Add(new VariableDeclarationAndAssignNode(tokens[i]));
                    }
                    else if (tokens[i].Type == TokenType.VariableRewriting)
                    {
                        node.Nodes.Add(new VariableAssignNode(tokens[i]));
                    }
                    else if (tokens[i].Type == TokenType.UserInput)
                    {
                        node.Nodes.Add(new UserInputNode(tokens[i]));
                    }
                    else if (tokens[i].Type == TokenType.UserOutput)
                    {
                        node.Nodes.Add(new UserOutputNode(tokens[i]));
                    }
                    else if (tokens[i].Type == TokenType.UserPrompt)
                    {
                        node.Nodes.Add(new UserPromptNode(tokens[i]));
                    }
                    else if (IsTokenWhiteSpace(tokens[i]))
                    {
                        continue;
                    }
                    else
                    {
                        compileErrors.Add(new Error($"Unexpected expression in {errorMsg} body - {tokens[i].Value}"));
                        return null;
                    }
                }
            }
            return node;
        }

        bool IsTokenWhiteSpace(Token token)
        {
            return token.Type == TokenType.Whitespace || token.Type == TokenType.SingleSpace || token.Type == TokenType.Newline || token.Type == TokenType.MultilineComment
                 || token.Type == TokenType.InlineComment;
        }
    }
}

using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.SyntaxNodes;
using System.Collections.Generic;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp
{
    public class Parser : IParser //TODO refactor this
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
            if (tokens.Count == 0)
            {
                compileErrors.Add(new Error("Empty program"));

                return null;
            }
            var output = Node(SyntaxType.Program, 0, tokens.Count, tokens, compileErrors);

            return output;
        }

        private SyntaxNode Node(SyntaxType parent, int start, int end, List<Token> tokens, List<Error> compileErrors)
        {
            switch (parent)
            {
                case SyntaxType.Program:
                    {
                        SyntaxNode node = new ProgramNode();
                        for (var i = start; i < end; i++)
                        {
                            switch (tokens[i].Type)
                            {
                                case TokenType.ClassDeclaration:
                                    {
                                        var newStart = i + 1;
                                        var left = 1;
                                        var right = 0;
                                        while (left != right)
                                        {
                                            i++;
                                            if (i >= end)
                                            {
                                                compileErrors.Add(new Error("Missing end of class declaration"));

                                                return null;
                                            }
                                            switch (tokens[i].Type)
                                            {
                                                case TokenType.ClassDeclaration:
                                                case TokenType.InterfaceDeclaration:
                                                    left++;
                                                    break;
                                                case TokenType.ClassEndDeclaration:
                                                    right++;
                                                    break;
                                            }
                                        }
                                        var childNode = Node(SyntaxType.Class, newStart, i, tokens, compileErrors);
                                        if (childNode == null)
                                        {
                                            return null;
                                        }
                                        node.Nodes.Add(childNode);
                                        break;
                                    }
                                case TokenType.InterfaceDeclaration:
                                    {
                                        var newStart = i + 1;
                                        var left = 1;
                                        var right = 0;
                                        while (left != right)
                                        {
                                            i++;
                                            if (i >= end)
                                            {
                                                compileErrors.Add(new Error("Missing end of interface declaration"));

                                                return null;
                                            }
                                            switch (tokens[i].Type)
                                            {
                                                case TokenType.ClassDeclaration:
                                                case TokenType.InterfaceDeclaration:
                                                    left++;
                                                    break;
                                                case TokenType.ClassEndDeclaration:
                                                    right++;
                                                    break;
                                            }
                                        }
                                        var childNode = Node(SyntaxType.Interface, newStart, i, tokens, compileErrors);
                                        if (childNode == null)
                                        {
                                            return null;
                                        }
                                        node.Nodes.Add(childNode);
                                        break;
                                    }
                                default:
                                    {
                                        if (IsTokenWhiteSpace(tokens[i]))
                                        {
                                            continue;
                                        }

                                        compileErrors.Add(new Error("Unexpected expression in program body"));
                                        return null;
                                    }
                            }
                        }
                        return node;
                    }
                case SyntaxType.Class:
                    {
                        SyntaxNode node = new ClassNode(tokens[start - 1], tokens[end]);
                        for (var i = start; i < end; i++)
                        {
                            switch (tokens[i].Type)
                            {
                                case TokenType.MethodDeclaration:
                                    {
                                        var newStart = i + 1;
                                        var left = 1;
                                        var right = 0;
                                        while (left != right)
                                        {
                                            i++;
                                            if (i >= end)
                                            {
                                                compileErrors.Add(new Error("Missing end of method declaration"));

                                                return null;
                                            }
                                            switch (tokens[i].Type)
                                            {
                                                case TokenType.MethodDeclaration:
                                                case TokenType.MainMethodDeclaration:
                                                    left++;
                                                    break;
                                                case TokenType.MethodEndDeclaration:
                                                    right++;
                                                    break;
                                            }
                                        }
                                        var childNode = Node(SyntaxType.MethodDeclaring, newStart, i, tokens, compileErrors);
                                        if (childNode == null)
                                        {
                                            return null;
                                        }
                                        node.Nodes.Add(childNode);
                                        break;
                                    }
                                case TokenType.MainMethodDeclaration:
                                    {
                                        var newStart = i + 1;
                                        var left = 1;
                                        var right = 0;
                                        while (left != right)
                                        {
                                            i++;
                                            if (i >= end)
                                            {
                                                compileErrors.Add(new Error("Missing end of main method declaration"));
                                                return null;
                                            }
                                            switch (tokens[i].Type)
                                            {
                                                case TokenType.MethodDeclaration:
                                                case TokenType.MainMethodDeclaration:
                                                    left++;
                                                    break;
                                                case TokenType.MethodEndDeclaration:
                                                    right++;
                                                    break;
                                            }
                                        }
                                        var childNode = Node(SyntaxType.MainMethodDeclaring, newStart, i, tokens, compileErrors);
                                        if (childNode == null)
                                        {
                                            return null;
                                        }
                                        node.Nodes.Add(childNode);
                                        break;
                                    }
                                default:
                                    {
                                        if (IsTokenWhiteSpace(tokens[i]))
                                        {
                                            continue;
                                        }

                                        compileErrors.Add(new Error("Unexpected expression in class body"));
                                        return null;
                                    }
                            }
                        }
                        return node;
                    }
                case SyntaxType.Interface:
                    {
                        SyntaxNode node = new InterfaceNode(tokens[start - 1], tokens[end]);
                        for (var i = start; i < end; i++)
                        {
                            if (tokens[i].Type == TokenType.MethodDeclaration)
                            {
                                node.Nodes.Add(new InterfaceMethodNode(tokens[i]));
                            }
                            else if (IsTokenWhiteSpace(tokens[i]))
                            {

                            }
                            else
                            {
                                compileErrors.Add(new Error("Unexpected expression in interface body"));

                                return null;
                            }
                        }
                        return node;
                    }
                case SyntaxType.MethodDeclaring:
                case SyntaxType.MainMethodDeclaring:
                    {
                        SyntaxNode node = new MethodNode(tokens[start - 1], tokens[end], parent == SyntaxType.MainMethodDeclaring);
                        Node(parent, start, end, tokens, compileErrors, node, "method");

                        return node;
                    }
                case SyntaxType.Switch:
                    {
                        SyntaxNode node = new SwitchNode(tokens[start - 1], tokens[end]);
                        Node(parent, start, end, tokens, compileErrors, node, "switch");

                        return node;
                    }
                case SyntaxType.SwitchCase:
                    {
                        SyntaxNode node = new SwitchCase(tokens[start - 1], tokens[start - 1].Type == TokenType.SwitchDefaultCase);
                        Node(parent, start, end, tokens, compileErrors, node, "switch case");

                        return node;
                    }
                case SyntaxType.IfTruePart:
                    {
                        SyntaxNode node = new IfTrueNode(tokens[start - 1].Childs[0]);
                        Node(parent, start, end, tokens, compileErrors, node, "if");

                        return node;
                    }
                case SyntaxType.IfFalsePart:
                    {
                        SyntaxNode node = new IfElseNode();
                        Node(parent, start, end, tokens, compileErrors, node, "else");

                        return node;
                    }
                case SyntaxType.ElifPart:
                    {
                        SyntaxNode node = new ElifNode(tokens[start - 1].Childs[0]);
                        Node(parent, start, end, tokens, compileErrors, node, "else-if");

                        return node;
                    }
                case SyntaxType.For:
                    {
                        SyntaxNode node = new ForNode(tokens[start - 1], tokens[end]);
                        Node(parent, start, end, tokens, compileErrors, node, "for");

                        return node;
                    }
                case SyntaxType.Foreach:
                    {
                        SyntaxNode node = new ForeachNode(tokens[start - 1], tokens[end]);
                        Node(parent, start, end, tokens, compileErrors, node, "foreach");

                        return node;
                    }
                case SyntaxType.While:
                    {
                        SyntaxNode node = new WhileNode(tokens[start - 1].Childs[0]);
                        Node(parent, start, end, tokens, compileErrors, node, "while");

                        return node;
                    }
                case SyntaxType.DoWhile:
                    {
                        SyntaxNode node = new DoWhileNode(tokens[end].Childs[0]);
                        Node(parent, start, end, tokens, compileErrors, node, "do-while");

                        return node;
                    }
                default:
                    {
                        compileErrors.Add(new Error("Not implemented parser behaviour"));

                        return null;
                    }
            }
        }

        private void Node(SyntaxType parent, int start, int end, List<Token> tokens, List<Error> compileErrors, SyntaxNode node, string errorMsg)
        {
            for (var i = start; i < end; i++)
            {
                if (parent == SyntaxType.Switch)
                {
                    switch (tokens[i].Type)
                    {
                        case TokenType.SwitchCase:
                            {
                                var newStart = i + 1;
                                var left = 1;
                                var right = 0;
                                while (left != right)
                                {
                                    i++;
                                    if (i >= end)
                                    {
                                        compileErrors.Add(new Error("Missing end of switch case statement"));

                                        return;
                                    }
                                    switch (tokens[i].Type)
                                    {
                                        case TokenType.SwitchCase:
                                            left++;
                                            break;
                                        case TokenType.SwitchCaseEnd:
                                            right++;
                                            break;
                                    }
                                }
                                var childNode = Node(SyntaxType.SwitchCase, newStart, i, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        case TokenType.SwitchDefaultCase:
                            {
                                var newStart = i + 1;
                                i = end - 1;
                                var childNode = Node(SyntaxType.SwitchCase, newStart, end, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        default:
                            {
                                if (IsTokenWhiteSpace(tokens[i]) || tokens[i].Type == TokenType.SwitchCaseEnd)
                                {
                                    continue;
                                }

                                compileErrors.Add(new Error($"Unexpected expression in {errorMsg} body - {tokens[i].Value}"));

                                return;
                            }
                    }
                }
                else
                {
                    switch (tokens[i].Type)
                    {
                        case TokenType.IfStart:
                            {
                                var newStart = i + 1;
                                var left = 1;
                                var right = 0;
                                while (left != right)
                                {
                                    i++;
                                    if (i >= end)
                                    {
                                        compileErrors.Add(new Error("Missing end of if statement"));

                                        return;
                                    }
                                    switch (tokens[i].Type)
                                    {
                                        case TokenType.IfStart:
                                        case TokenType.IfElse:
                                        case TokenType.Elif:
                                            left++;
                                            break;
                                        case TokenType.IfEnd:
                                            right++;
                                            break;
                                    }
                                }
                                var childNode = Node(SyntaxType.IfTruePart, newStart, i, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        case TokenType.Elif:
                            {
                                var newStart = i + 1;
                                var left = 1;
                                var right = 0;
                                while (left != right)
                                {
                                    i++;
                                    if (i >= end)
                                    {
                                        compileErrors.Add(new Error("Missing end of else part of if statement"));

                                        return;
                                    }
                                    switch (tokens[i].Type)
                                    {
                                        case TokenType.IfStart:
                                        case TokenType.IfElse:
                                        case TokenType.Elif:
                                            left++;
                                            break;
                                        case TokenType.IfEnd:
                                            right++;
                                            break;
                                    }
                                }
                                var childNode = Node(SyntaxType.ElifPart, newStart, i, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        case TokenType.IfElse:
                            {
                                var newStart = i + 1;
                                var left = 1;
                                var right = 0;
                                while (left != right)
                                {
                                    i++;
                                    if (i >= end)
                                    {
                                        compileErrors.Add(new Error("Missing end of else part of if statement"));

                                        return;
                                    }
                                    switch (tokens[i].Type)
                                    {
                                        case TokenType.IfStart:
                                        case TokenType.IfElse:
                                        case TokenType.Elif:
                                            left++;
                                            break;
                                        case TokenType.IfEnd:
                                            right++;
                                            break;
                                    }
                                }
                                var childNode = Node(SyntaxType.IfFalsePart, newStart, i, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        case TokenType.ForStart:
                        case TokenType.ForStartWithDeclaring:
                            {
                                var newStart = i + 1;
                                var left = 1;
                                var right = 0;
                                while (left != right)
                                {
                                    i++;
                                    if (i >= end)
                                    {
                                        compileErrors.Add(new Error("Missing end of for statement"));

                                        return;
                                    }
                                    switch (tokens[i].Type)
                                    {
                                        case TokenType.ForStart:
                                        case TokenType.ForStartWithDeclaring:
                                        case TokenType.ForeachStart:
                                        case TokenType.ForeachStartWithDeclaring:
                                        case TokenType.SwitchDeclaration:
                                        case TokenType.WhileStart:
                                            left++;
                                            break;
                                        case TokenType.CycleEnding:
                                            right++;
                                            break;
                                    }
                                }
                                var childNode = Node(SyntaxType.For, newStart, i, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        case TokenType.ForeachStart:
                        case TokenType.ForeachStartWithDeclaring:
                            {
                                var newStart = i + 1;
                                var left = 1;
                                var right = 0;
                                while (left != right)
                                {
                                    i++;
                                    if (i >= end)
                                    {
                                        compileErrors.Add(new Error("Missing end of foreach statement"));

                                        return;
                                    }
                                    switch (tokens[i].Type)
                                    {
                                        case TokenType.ForStart:
                                        case TokenType.ForStartWithDeclaring:
                                        case TokenType.ForeachStart:
                                        case TokenType.ForeachStartWithDeclaring:
                                        case TokenType.SwitchDeclaration:
                                        case TokenType.WhileStart:
                                            left++;
                                            break;
                                        case TokenType.CycleEnding:
                                            right++;
                                            break;
                                    }
                                }
                                var childNode = Node(SyntaxType.Foreach, newStart, i, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        case TokenType.WhileStart:
                            {
                                var newStart = i + 1;
                                var left = 1;
                                var right = 0;
                                while (left != right)
                                {
                                    i++;
                                    if (i >= end)
                                    {
                                        compileErrors.Add(new Error("Missing end of while statement"));

                                        return;
                                    }
                                    switch (tokens[i].Type)
                                    {
                                        case TokenType.ForStart:
                                        case TokenType.ForStartWithDeclaring:
                                        case TokenType.ForeachStart:
                                        case TokenType.ForeachStartWithDeclaring:
                                        case TokenType.SwitchDeclaration:
                                        case TokenType.WhileStart:
                                            left++;
                                            break;
                                        case TokenType.CycleEnding:
                                            right++;
                                            break;
                                    }
                                }
                                var childNode = Node(SyntaxType.While, newStart, i, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        case TokenType.DoWhileStart:
                            {
                                var newStart = i + 1;
                                var left = 1;
                                var right = 0;
                                while (left != right)
                                {
                                    i++;
                                    if (i >= end)
                                    {
                                        compileErrors.Add(new Error("Missing end of do while statement"));

                                        return;
                                    }
                                    switch (tokens[i].Type)
                                    {
                                        case TokenType.DoWhileStart:
                                            left++;
                                            break;
                                        case TokenType.DoWhileEnd:
                                            right++;
                                            break;
                                    }
                                }
                                var childNode = Node(SyntaxType.DoWhile, newStart, i, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        case TokenType.SwitchDeclaration:
                            {
                                var newStart = i + 1;
                                var left = 1;
                                var right = 0;
                                while (left != right)
                                {
                                    i++;
                                    if (i >= end)
                                    {
                                        compileErrors.Add(new Error("Missing end of switch statement"));

                                        return;
                                    }
                                    switch (tokens[i].Type)
                                    {
                                        case TokenType.ForStart:
                                        case TokenType.ForStartWithDeclaring:
                                        case TokenType.ForeachStart:
                                        case TokenType.ForeachStartWithDeclaring:
                                        case TokenType.SwitchDeclaration:
                                        case TokenType.WhileStart:
                                            left++;
                                            break;
                                        case TokenType.CycleEnding:
                                            right++;
                                            break;
                                    }
                                }
                                var childNode = Node(SyntaxType.Switch, newStart, i, tokens, compileErrors);
                                if (childNode == null)
                                {
                                    return;
                                }
                                node.Nodes.Add(childNode);
                                break;
                            }
                        case TokenType.MethodCalling:
                            node.Nodes.Add(new MethodCalling(tokens[i]));
                            break;
                        case TokenType.MethodReturn:
                            node.Nodes.Add(new MethodReturn(tokens[i]));
                            break;
                        case TokenType.VariableDeclaration:
                            node.Nodes.Add(new VariableDeclarationNode(tokens[i]));
                            break;
                        case TokenType.VariableDeclarationAndAssign:
                            node.Nodes.Add(new VariableDeclarationAndAssignNode(tokens[i]));
                            break;
                        case TokenType.VariableRewriting:
                            node.Nodes.Add(new VariableAssignNode(tokens[i]));
                            break;
                        case TokenType.UserInput:
                            node.Nodes.Add(new UserInputNode(tokens[i]));
                            break;
                        case TokenType.UserOutput:
                            node.Nodes.Add(new UserOutputNode(tokens[i]));
                            break;
                        case TokenType.UserPrompt:
                            node.Nodes.Add(new UserPromptNode(tokens[i]));
                            break;
                        default:
                            {
                                if (IsTokenWhiteSpace(tokens[i]))
                                {
                                    continue;
                                }

                                compileErrors.Add(new Error($"Unexpected expression in {errorMsg} body - {tokens[i].Value}"));

                                return;
                            }
                    }
                }
            }
        }

        private static bool IsTokenWhiteSpace(Token token)
        {
            return token.Type == TokenType.Whitespace || token.Type == TokenType.SingleSpace || token.Type == TokenType.Newline || token.Type == TokenType.MultilineComment
                 || token.Type == TokenType.InlineComment;
        }
    }
}

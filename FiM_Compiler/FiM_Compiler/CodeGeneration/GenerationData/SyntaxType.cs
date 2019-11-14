using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public enum SyntaxType
    {
        Program, Class, Interface, MethodDeclaring, MainMethodDeclaring, InterfaceMethodDeclaring, 
        IfTruePart, IfFalsePart, ElifPart,
        Switch, SwitchCase, DoWhile, While, For, Foreach,
        Expression, MethodCalling, MethodReturn,
        VariableDeclaration, VariableDeclarationAndAssign, VariableAssign,
        UserInput, UserOutput, UserPrompt,
    }
}

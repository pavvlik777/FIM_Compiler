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

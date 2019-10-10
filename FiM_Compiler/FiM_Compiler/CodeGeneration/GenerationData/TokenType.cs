namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public enum TokenType
    {
        Char, Punctuation, Name, Keyword, Whitespace, SingleSpace, VariableType, Newline,
        InlineComment, OpenMultilineComments, CloseMultilineComments,
        ClassName, MainSuperclassName, ClassExtendsName, InterfaceName, MethodName, VariableName, MethodVariableName, IntLiteral, CharLiteral, StringLiteral, BoolLiteral, NullLiteral,
        VariableDeclaration, VariableDeclarationAndAssign, ClassDeclaration, InterfaceDeclaration, ClassEndDeclaration, ClassExtends,
        MainMethodDeclaration, MethodDeclaration, MethodParameters, MethodParametersExtra, MethodEndDeclaration,
        MethodReturn, MethodCalling, SeparateMethodCalling, MethodCallingParameters, MethodCallingParametersExtra
    }
}

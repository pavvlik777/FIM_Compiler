namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public enum TokenType
    {
        Char, Punctuation, Name, Keyword, Whitespace, SingleSpace, VariableType, Newline,
        InlineComment, OpenMultilineComments, CloseMultilineComments,
        ClassName, InterfaceName, MethodName, VariableName, VariableDeclaration, VariableDeclarationAndAssign, Literal,
        ClassDeclaration, InterfaceDeclaration, ClassEndDeclaration, ClassExtends,
        MainMethodDeclaration, MethodDeclaration, MethodParameters, MethodParametersExtra, MethodEndDeclaration,
        MethodReturn, MethodCalling, SeparateMethodCalling, MethodCallingParameters, MethodCallingParametersExtra
    }
}

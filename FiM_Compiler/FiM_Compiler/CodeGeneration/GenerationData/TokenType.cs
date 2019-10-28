namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public enum TokenType
    {
        Char, Punctuation, Name, Keyword, Whitespace, SingleSpace, VariableType, Newline,
        InlineCommentStart, OpenMultilineComments, CloseMultilineComments, InlineComment, MultilineComment,
        ClassName, MainSuperclassName, ClassExtendsName, InterfaceName, MethodName, VariableName, MethodVariableName,
        IntLiteral, CharLiteral, StringLiteral, BoolLiteral, NullLiteral, 
        Literal, Value, BoolValue, IntValue,
        VariableDeclaration, VariableDeclarationAndAssign, ClassDeclaration, InterfaceDeclaration, ClassEndDeclaration, ClassExtends, ClassMainPart, InterfaceMainPart,
        MainMethodDeclaration, MethodDeclaration, MethodParameters, MethodParametersExtra, MethodEndDeclaration,
        MethodReturn, MethodCalling, SeparateMethodCalling, MethodCallingParameters, MethodCallingParametersExtra,
        ArifmeticExpression, ArifmeticAddition, ArifmeticSubstraction, ArifmeticMultiplication, ArifmeticDivision, ArifmeticIncrement, ArifmeticDecrement,
        BooleanExpression, IsEqual, IsNotEqual, LessThan, GreaterThan, LessThanOrEqual, GreaterThanOrEqual,
        UserInput, UserPrompt, UserOutput, VariableRewriting,
        BooleanAnd, BooleanOr, BooleanXor, BooleanNot,
        IfStart, IfEnd, IfElse, SwitchDeclaration, SwitchCase, SwitchDefaultCase, CycleEnding,
        WhileStart, DoWhileStart, DoWhileEnd, ForStart, ForeachStart,
    }
}

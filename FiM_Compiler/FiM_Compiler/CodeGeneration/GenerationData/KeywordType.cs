namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public enum KeywordType
    {
        Unknown,
        ClassDeclaration, ClassEndDeclaration, ClassImplements, InterfaceDeclaration,
        MethodCalling, MethodCallingParameters, MethodCallingParametersExtra, MethodReturn,
        MainMethodDeclaration, MethodDeclaration, MethodDeclarationReturn, MethodDeclarationParameters, MethodDeclarationParametersExtra, MethodEndDeclaration,
        VariableDeclaration, VariableDeclarationSecond,

        ArifmeticAddition, ArifmeticAdditionPrefixFirst, ArifmeticAdditionPrefixSecond,
        ArifmeticSubstraction, ArifmeticSubstractionPrefixFirst, ArifmeticSubstractionPrefixSecond,
        ArifmeticMultiplication, ArifmeticMultiplicationPrefixFirst, ArifmeticMultiplicationPrefixSecond,
        ArifmeticDivision, ArifmeticDivisionPrefixFirst, ArifmeticDivisionPrefixSecond,
        ArifmeticIncrement, ArifemticDecrement,

        BooleanComparator, BooleanComparatorNot, BooleanLessThan, BooleanGreaterThan,
        BooleanAnd, BooleanOr, BooleanXor, BooleanNot,

        UserInput, UserPrompt, UserOutput,

        Int, Char, String, Bool, IntArray, CharArray, StringArray, BoolArray
    }
}

using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ClassesAndInterfaces;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.MethodsDeclaration;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.StatementsAndLoops;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.Variables;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalyses
{
    public class KeywordDeclarationAnalysis : ILexerAnalysis
    {
        private List<TokenRule> declarationRules;

        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            var initStack = new List<Token>(tokens);
            var stack = new List<Token>();
            var i = 0;
            while (i < initStack.Count)
            {
                if (!CheckStackForPatterns(stack, declarationRules))
                {
                    stack.Add(initStack[i]);
                    i++;
                }
            }
            return stack;
        }

        private bool CheckStackForPatterns(List<Token> tokens, List<TokenRule> rules)
        {
            var output = false;
            foreach (var cur in rules)
                if (cur.IsStackMatch(tokens))
                {
                    output = true;
                    break;
                }
            return output;
        }

        #region Constructor
        public KeywordDeclarationAnalysis()
        {
            declarationRules = new List<TokenRule>()
            {
                new ClassMainPartRule(), new InterfaceMainPartRule(), new ClassExtendsRule(), new ClassExtendsMergeRule(),
                new ClassDeclarationRule(), new ClassDeclarationWithExtendsRule(),
                new InterfaceDeclaration(), new InterfaceDeclarationWithExtends(),
                new ClassEndDeclaration(),

                new MainMethodDeclaration(),
                new MethodDeclaration(), new MethodWithParametersDeclaration(), new MethodWithReturnDeclaration(), new MethodWithReturnAndParametersDeclaration(),
                new MethodParameters(), new MethodParametersExtra(), new MethodParametersMerge(),
                new EndMethodDeclaration(),

                new VariableDeclaration(), new VariableDeclarationAndAssignValue(), new VariableDeclarationWithType(),
                new ForStartWithDeclaring(),
                new ForeachStartWithDeclaring(),
            };
            Sort(declarationRules);
        }

        private void Sort(List<TokenRule> rules)
        {
            for (var i = 0; i < rules.Count - 1; i++) // Comment this if need specific order
            {
                for (var j = i + 1; j < rules.Count; j++)
                {
                    if (rules[i].Amount < rules[j].Amount)
                    {
                        var temp = rules[i];
                        rules[i] = rules[j];
                        rules[j] = temp;
                    }
                }
            }
        }
        #endregion
    }
}

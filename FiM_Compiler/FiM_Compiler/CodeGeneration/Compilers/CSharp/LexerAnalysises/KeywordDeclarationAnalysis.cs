using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalysises
{
    public class KeywordDeclarationAnalysis : ILexerAnalysis
    {
        List<TokenRule> declarationRules;

        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            List<Token> initStack = new List<Token>(tokens);
            List<Token> stack = new List<Token>();
            int i = 0;
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

        bool CheckStackForPatterns(List<Token> tokens, List<TokenRule> rules)
        {
            bool output = false;
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

                new VariableDeclaration(), new VariableDeclarationAndAssignValue(), new VariableDeclarationWithType()
            };
            Sort(declarationRules);
        }

        void Sort(List<TokenRule> rules)
        {
            for (int i = 0; i < rules.Count - 1; i++) // Comment this if need specific order
            {
                for (int j = i + 1; j < rules.Count; j++)
                {
                    if (rules[i].Amount < rules[j].Amount)
                    {
                        TokenRule temp = rules[i];
                        rules[i] = rules[j];
                        rules[j] = temp;
                    }
                }
            }
        }
        #endregion
    }
}

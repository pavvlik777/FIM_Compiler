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
                if (!CheckStackForPatterns(ref stack, declarationRules))
                {
                    stack.Add(initStack[i]);
                    i++;
                }
            }
            return stack;
        }

        bool CheckStackForPatterns(ref List<Token> tokens, List<TokenRule> rules)
        {
            bool output = false;
            foreach (var cur in rules)
                if (cur.IsStackMatch(ref tokens))
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
                new InlineCommentStartRule(), new InlineCommentMergeRule(),

                new ClassExtendsRule(), new ClassExtendsMergeRule(), new ClassDeclarationRule(), new ClassDeclarationWithExtendsRule(), new ClassEndDeclaration(),
                new InterfaceDeclaration(), new InterfaceDeclarationWithExtends(),

                new MainMethodDeclaration(), new MethodDeclaration(), new MethodWithParametersDeclaration(), new MethodWithReturnAndParametersDeclaration(), new MethodWithReturnDeclaration(),
                new MethodParameters(), new MethodParametersExtra(), new MethodParametersMerge(), new EndMethodDeclaration(),

                new VariableDeclaration(), new VariableDeclarationAndAssignValue(), new VariableDeclarationWithType()
            };
            Sort(ref declarationRules);
        }

        void Sort(ref List<TokenRule> rules)
        {
            for (int i = 0; i < rules.Count - 1; i++) // Comment this if need specific order
            {
                for (int j = i + 1; j < rules.Count; j++)
                {
                    if (declarationRules[i].Amount < declarationRules[j].Amount)
                    {
                        TokenRule temp = declarationRules[i];
                        declarationRules[i] = declarationRules[j];
                        declarationRules[j] = temp;
                    }
                }
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.BooleanOperators;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.BooleanOperations;
using FiM_Compiler.CodeGeneration.GenerationData.KeywordTokenRules.ArifmeticOperations;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalysises
{
    public class ArifmeticAnalysis : ILexerAnalysis
    {
        List<TokenRule> rules;

        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            List<Token> initStack = new List<Token>(tokens);
            List<Token> stack = new List<Token>();
            int i = 0;
            while (i < initStack.Count)
            {
                if (!CheckStackForPatterns(stack, rules))
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
        public ArifmeticAnalysis()
        {
            rules = new List<TokenRule>()
            {
                new AdditionInfix(), new AdditionPrefix(), new ArifmeticIncrement(),
                new SubstractionInfix(), new SubstractionPrefix(), new ArifmeticDecrement(),
                new MultiplicationInfix(), new MultiplicationPrefix(),
                new DivisionInfix(), new DivisionPrefix(),
                new ArifmeticExpression(), new BooleanComparsionsExpression(), new BooleanOperatorsExpression(),
                new IsEqual(), new IsNotEqual(),
                new IsLessThan(), new IsLessThanOrEqual(), new IsGreaterThan(), new IsGreaterThanOrEqual(),
                new BooleanAnd(), new BooleanOr(), new BooleanNot(), new BooleanXor()
            };
            Sort(rules);
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

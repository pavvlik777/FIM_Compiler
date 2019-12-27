using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.InitialTokenRules;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalyses
{
    public class InitialAnalysis : ILexerAnalysis
    {
        string[] keywords;
        string[] variablesTypes;
        char[] punctuation = { ':', '.', '!', ',', '?' };

        List<TokenRule> rules;

        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            var initStack = new List<Token>();
            var stack = new List<Token>();
            var i = 0;
            Token newToken = null;
            while(i < sourceCode.Length)
            {
                if(!CheckStackForPatterns(ref initStack, rules))
                {
                    newToken = GetNextToken(sourceCode[i]);
                    initStack.Add(newToken);
                    i++;
                }
            }
            i = 0;
            while (i < initStack.Count)
            {
                if (!CheckStackForKeyword(ref stack))
                {
                    stack.Add(initStack[i]);
                    i++;
                }
            }
            initStack.Clear();
            initStack = new List<Token>(stack);
            stack.Clear();
            i = 0;
            while (i < initStack.Count)
            {
                if (!CheckStackForVariable(ref stack))
                {
                    stack.Add(initStack[i]);
                    i++;
                }
            }

            return stack;
        }

        #region TokenMethods
        bool CheckStackForPatterns(ref List<Token> tokens, List<TokenRule> rules)
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

        bool IsKeyword(string input)
        {
            foreach (var keyword in keywords)
            {
                var temp = keyword.Split(' ');
                foreach (var cur in temp)
                    if (input == cur)
                        return true;
            }
            return false;
        }

        bool IsPunctuation(char input)
        {
            foreach (var cur in punctuation)
                if (input == cur)
                    return true;
            return false;
        }

        Token GetNextToken(char input)
        {
            Token newToken = null;
            if (IsPunctuation(input))
                newToken = new Token(TokenType.Punctuation, input.ToString());
            else if (input == '\n')
                newToken = new Token(TokenType.Newline, input.ToString());
            else if (input == '(')
                newToken = new Token(TokenType.OpenMultilineComments, input.ToString());
            else if (input == ')')
                newToken = new Token(TokenType.CloseMultilineComments, input.ToString());
            else if (input == ' ')
                newToken = new Token(TokenType.SingleSpace, input.ToString());
            else if (input == '"')
                newToken = new Token(TokenType.Delimeters, input.ToString());
            else if (IsWhitespace(input))
                newToken = new Token(TokenType.Whitespace, input.ToString());
            else
                newToken = new Token(TokenType.Char, input.ToString());
            return newToken;
        }

        bool IsWhitespace(char symbol)
        {
            return symbol == ' ' || symbol == '\t';
        }

        bool CheckStackForVariable(ref List<Token> stack)
        {
            foreach (var keyword in variablesTypes)
            {
                var template = keyword.Split(' ');
                var i = template.Length * 2 - 1;
                while (stack.Count >= i && i > 0)
                {
                    var temp = "";
                    for (var j = i; j >= 1; j--)
                    {
                        temp += stack[stack.Count - j].Value;
                    }
                    if (temp == keyword)
                    {
                        if (i != 1 || stack[stack.Count - 1].Type != TokenType.VariableType)
                        {
                            //for (int i = 1; i <= template.Length * 2 - 1; i = i + 2)
                            //    stack[stack.Count - i].Type = TokenType.Keyword;
                            ConvertTokens(ref stack, i, TokenType.VariableType); //this code makes value like => Your faithful student
                            return true;
                        }
                    }
                    i--;
                }
            }
            return false;
        }

        void ConvertTokens(ref List<Token> stack, int amount, TokenType newType, List<Token> childs = null)
        {
            var value = "";
            Token newToken = null;
            for (var j = amount; j >= 1; j--)
                value += stack[stack.Count - j].Value;
            if (childs != null)
                newToken = new Token(newType, value, new List<Token>(childs));
            else
                newToken = new Token(newType, value);
            for (var j = 1; j <= amount; j++)
                stack.RemoveAt(stack.Count - 1);
            stack.Add(newToken);
        }

        bool CheckStackForKeyword(ref List<Token> stack)
        {
            foreach (var keyword in keywords)
            {
                var template = keyword.Split(' ');
                var i = template.Length * 2 - 1;
                while (stack.Count >= i && i > 0)
                {
                    var temp = "";
                    for (var j = i; j >= 1; j--)
                    {
                        temp += stack[stack.Count - j].Value;
                    }
                    if (temp == keyword)
                    {
                        if(i != 1 || stack[stack.Count - 1].Type != TokenType.Keyword)
                        {
                            //for (int i = 1; i <= template.Length * 2 - 1; i = i + 2)
                            //    stack[stack.Count - i].Type = TokenType.Keyword;
                            ConvertTokens(ref stack, i, TokenType.Keyword); //this code makes value like => Your faithful student
                            return true;
                        }
                    }
                    i--;
                }
            }
            return false;
        }
        #endregion

        #region Constructor
        public InitialAnalysis()
        {
            rules = new List<TokenRule>()
            {
                new CharToNameRule(), new NameMergeRule(), new WhiteSpaceMergeRule()
            };
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
            keywords = KeywordsDictionary.GetKeywords().ToArray();
            variablesTypes = KeywordsDictionary.GetVariableTypes().ToArray();
            Sort(ref keywords);
            Sort(ref variablesTypes);
        }

        void Sort(ref string[] keywords)
        {
            for (var i = 0; i < keywords.Length - 1; i++)
            {
                for (var j = i + 1; j < keywords.Length; j++)
                {
                    var first = 0;
                    for (var t = 0; t < keywords[i].Length; t++)
                        if (keywords[i][t] == ' ')
                            first++;
                    var second = 0;
                    for (var t = 0; t < keywords[j].Length; t++)
                        if (keywords[j][t] == ' ')
                            second++;
                    if (first < second)
                    {
                        var temp = keywords[i];
                        keywords[i] = keywords[j];
                        keywords[j] = temp;
                    }
                }
            }
        }
        #endregion
    }
}

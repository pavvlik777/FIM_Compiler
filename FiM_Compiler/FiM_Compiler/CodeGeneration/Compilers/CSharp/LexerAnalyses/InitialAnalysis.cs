using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.Compilers.Interfaces;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.InitialTokenRules;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalyses
{
    public class InitialAnalysis : ILexerAnalysis
    {
        private readonly string[] _keywords;
        private readonly string[] _variablesTypes;
        private readonly char[] _punctuation = { ':', '.', '!', ',', '?' };

        private readonly List<TokenRule> _rules;

        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            var initStack = new List<Token>();
            var stack = new List<Token>();
            var i = 0;
            while(i < sourceCode.Length)
            {
                if(!CheckStackForPatterns(ref initStack, _rules))
                {
                    var newToken = GetNextToken(sourceCode[i]);
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

        private bool CheckStackForPatterns(ref List<Token> tokens, List<TokenRule> rules)
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

        private bool IsKeyword(string input)
        {
            foreach (var keyword in _keywords)
            {
                var temp = keyword.Split(' ');
                foreach (var cur in temp)
                    if (input == cur)
                        return true;
            }
            return false;
        }

        private bool IsPunctuation(char input)
        {
            foreach (var cur in _punctuation)
                if (input == cur)
                    return true;
            return false;
        }

        private Token GetNextToken(char input)
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

        private bool IsWhitespace(char symbol)
        {
            return symbol == ' ' || symbol == '\t';
        }

        private bool CheckStackForVariable(ref List<Token> stack)
        {
            foreach (var keyword in _variablesTypes)
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

        private void ConvertTokens(ref List<Token> stack, int amount, TokenType newType, List<Token> childs = null)
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

        private bool CheckStackForKeyword(ref List<Token> stack)
        {
            foreach (var keyword in _keywords)
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
            _rules = new List<TokenRule>()
            {
                new CharToNameRule(), new NameMergeRule(), new WhiteSpaceMergeRule()
            };
            for (var i = 0; i < _rules.Count - 1; i++) // Comment this if need specific order
            {
                for (var j = i + 1; j < _rules.Count; j++)
                {
                    if (_rules[i].Amount < _rules[j].Amount)
                    {
                        var temp = _rules[i];
                        _rules[i] = _rules[j];
                        _rules[j] = temp;
                    }
                }
            }
            _keywords = KeywordsDictionary.GetKeywords().ToArray();
            _variablesTypes = KeywordsDictionary.GetVariableTypes().ToArray();
            Sort(ref _keywords);
            Sort(ref _variablesTypes);
        }

        private void Sort(ref string[] keywords)
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

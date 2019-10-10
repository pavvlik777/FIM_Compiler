using System;
using System.Collections.Generic;
using FiM_Compiler.CodeGeneration.GenerationData;
using FiM_Compiler.CodeGeneration.GenerationData.InitialTokenRules;

namespace FiM_Compiler.CodeGeneration.Compilers.CSharp.LexerAnalysises
{
    public class InitialAnalysis : ILexerAnalysis
    {
        string[] keywords = { "Dear", "Dearest", "Your faithful student", "I learned", "Today", "That's all about", "and",
        "with", "to get", "using", "Then you get", "Did you know that",
        "is", "was", "has", "had", "like", "likes", "liked",
        "I said"};
        string[] variablesTypes = { "the number", "a number", "number", "numbers", "many numbers", "character", "a character", "the character", "letter", "a letter", "the letter",
            "many letters", "many characters", "letters", "many characters", "word", "phrase", "sentence", "quote", "name", "a word", "a phrase", "a sentence", "a quote", "a name",
            "the word", "the phrase", "the sentence", "the quote", "the name", "many words", "many phrases", "many sentences", "many quotes", "many names", "logic", "argument",
            "a logic", "an argument", "the logic", "the argument" };
        char[] punctuation = { ':', '.', '!', ',', '?' };

        List<TokenRule> rules;
        List<TokenRule> mergeNamesRules;

        public List<Token> PerformLexicalAnalysis(List<Token> tokens, string sourceCode)
        {
            List<Token> initStack = new List<Token>();
            List<Token> stack = new List<Token>();
            int i = 0;
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
            initStack.Clear();
            initStack = new List<Token>(stack);
            stack.Clear();
            i = 0;
            while (i < initStack.Count)
            {
                if (!CheckStackForPatterns(ref stack, mergeNamesRules))
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
            bool output = false;
            foreach (var cur in rules)
                if (cur.IsStackMatch(ref tokens))
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
                string[] temp = keyword.Split(' ');
                foreach (var cur in temp)
                    if (input == cur)
                        return true;
            }
            return false;
        }

        bool IsPunctuation(char input)
        {
            foreach (var cur in punctuation)
            {
                if (input == cur)
                    return true;
            }
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
            bool output = false;
            foreach (string keyword in variablesTypes)
            {
                string[] template = keyword.Split(' ');
                if (stack.Count >= template.Length * 2 - 1)
                {
                    output = true;
                    for (int i = 1; i <= template.Length * 2 - 1; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (stack[stack.Count - i].Type != TokenType.SingleSpace)
                            {
                                output = false;
                                break;
                            }
                        }
                        else
                        {
                            if (stack[stack.Count - i].Type != TokenType.Name || stack[stack.Count - i].Value != template[template.Length - (i + 1) / 2])
                            {
                                output = false;
                                break;
                            }
                        }
                    }
                    if (output)
                    {
                        ConvertTokens(ref stack, template.Length * 2 - 1, TokenType.VariableType); //this code makes value like => Your faithful student
                        return true;
                    }
                }
            }
            return output;
        }

        void ConvertTokens(ref List<Token> stack, int amount, TokenType newType, List<Token> childs = null)
        {
            string value = "";
            Token newToken = null;
            for (int j = amount; j >= 1; j--)
                value += stack[stack.Count - j].Value;
            if (childs != null)
                newToken = new Token(newType, value, new List<Token>(childs));
            else
                newToken = new Token(newType, value);
            for (int j = 1; j <= amount; j++)
                stack.RemoveAt(stack.Count - 1);
            stack.Add(newToken);
        }

        bool CheckStackForKeyword(ref List<Token> stack)
        {
            bool output = false;
            foreach (string keyword in keywords)
            {
                string[] template = keyword.Split(' ');
                if (stack.Count >= template.Length * 2 - 1)
                {
                    output = true;
                    for (int i = 1; i <= template.Length * 2 - 1; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (stack[stack.Count - i].Type != TokenType.SingleSpace)
                            {
                                output = false;
                                break;
                            }
                        }
                        else
                        {
                            if (stack[stack.Count - i].Type != TokenType.Name || stack[stack.Count - i].Value != template[template.Length - (i + 1) / 2])
                            {
                                output = false;
                                break;
                            }
                        }
                    }
                    if (output)
                    {
                        //for (int i = 1; i <= template.Length * 2 - 1; i = i + 2)
                        //    stack[stack.Count - i].Type = TokenType.Keyword;
                        ConvertTokens(ref stack, template.Length * 2 - 1, TokenType.Keyword); //this code makes value like => Your faithful student
                        return true;
                    }
                }
            }
            return output;
        }
        #endregion

        #region Constructor
        public InitialAnalysis()
        {
            rules = new List<TokenRule>()
            {
                new CharToNameRule(), new NameMergeRule(), new WhiteSpaceMergeRule()
            };
            mergeNamesRules = new List<TokenRule>()
            {
                new NameWithWhitespaceMergeRule()
            };
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
            Sort(ref keywords);
            Sort(ref variablesTypes);
        }

        void Sort(ref string[] keywords)
        {
            for (int i = 0; i < keywords.Length - 1; i++)
            {
                for (int j = i + 1; j < keywords.Length; j++)
                {
                    int first = 0;
                    for (int t = 0; t < keywords[i].Length; t++)
                        if (keywords[i][t] == ' ')
                            first++;
                    int second = 0;
                    for (int t = 0; t < keywords[j].Length; t++)
                        if (keywords[j][t] == ' ')
                            second++;
                    if (first < second)
                    {
                        string temp = keywords[i];
                        keywords[i] = keywords[j];
                        keywords[j] = temp;
                    }
                }
            }
        }
        #endregion
    }
}

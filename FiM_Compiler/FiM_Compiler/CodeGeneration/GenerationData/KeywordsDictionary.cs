using System;
using System.Collections.Generic;
using System.Linq;

namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public static class KeywordsDictionary
    {
        private static Dictionary<KeywordType, string[]> keywordsDictionary = new Dictionary<KeywordType, string[]>()
        {
            { KeywordType.ClassDeclaration, new string[] { "Dear" } },
            { KeywordType.InterfaceDeclaration, new string[] { "Dearest" } },
            { KeywordType.ClassImplements, new string[] { "and" } },
            { KeywordType.ClassEndDeclaration, new string[] { "Your faithful student" } },

            { KeywordType.MethodCalling, new string[] { "I said", "I remembered", "I would" } },
            { KeywordType.MethodCallingParameters, new string[] { "using" } },
            { KeywordType.MethodCallingParametersExtra, new string[] { "and" } },
            { KeywordType.MethodReturn, new string[] { "Then you get" } },

            { KeywordType.MainMethodDeclaration, new string[] { "Today" } },
            { KeywordType.MethodDeclaration, new string[] { "I learned" } },
            { KeywordType.MethodDeclarationReturn, new string[] { "with", "to get" } },
            { KeywordType.MethodDeclarationParameters, new string[] { "using" } },
            { KeywordType.MethodDeclarationParametersExtra, new string[] { "and" } },
            { KeywordType.MethodEndDeclaration, new string[] { "That's all about" } },

            { KeywordType.VariableDeclaration, new string[] { "Did you know that" } },
            { KeywordType.VariableDeclarationSecond, new string[] { "is", "was", "has", "had", "like", "likes", "liked" } },

            { KeywordType.ArifmeticAddition, new string[] { "plus", "added to", "and" } },
            { KeywordType.ArifmeticAdditionPrefixFirst, new string[] { "add" } },
            { KeywordType.ArifmeticAdditionPrefixSecond, new string[] { "and" } },

            { KeywordType.ArifmeticSubstraction, new string[] { "minus", "without" } },
            { KeywordType.ArifmeticSubstractionPrefixFirst, new string[] { "substract", "the difference between" } },
            { KeywordType.ArifmeticSubstractionPrefixSecond, new string[] { "and" } },

            { KeywordType.ArifmeticMultiplication, new string[] { "times", "multiplied with" } },
            { KeywordType.ArifmeticMultiplicationPrefixFirst, new string[] { "multiply" } },
            { KeywordType.ArifmeticMultiplicationPrefixSecond, new string[] { "and" } },

            { KeywordType.ArifmeticDivision, new string[] { "divided by", "divide" } },
            { KeywordType.ArifmeticDivisionPrefixFirst, new string[] { "divide" } },
            { KeywordType.ArifmeticDivisionPrefixSecond, new string[] { "and", "by" } },

            { KeywordType.ArifmeticIncrement, new string[] { "got one more" } },
            { KeywordType.ArifemticDecrement, new string[] { "got one less" } },

            { KeywordType.UserInput, new string[] { "I heard", "I read", "I asked" } },
            { KeywordType.UserPrompt, new string[] { "I asked" } },
            { KeywordType.UserOutput, new string[] { "I said", "I wrote", "I sang", "I thought" } },
            { KeywordType.VariableRewriting, new string[] { "is now", "are now", "become", "becomes", "now become", "now becomes", "now like", "now likes"} },

            { KeywordType.BooleanComparator, new string[] { "is", "was", "has", "had" } },
            { KeywordType.BooleanComparatorNot, new string[] { "is not", "was not", "has not", "had not", "isn't", "wasn't", "hasn't", "hadn't" } },
            { KeywordType.BooleanLessThan, new string[] { "less than" } },
            { KeywordType.BooleanGreaterThan, new string[] { "more than", "greater than" } },
            
            { KeywordType.BooleanAnd, new string[] { "and" } },
            { KeywordType.BooleanOr, new string[] { "or" } },
            { KeywordType.BooleanXor, new string[] { "either" } },
            { KeywordType.BooleanNot, new string[] { "not" } },

            { KeywordType.IfStartFirst, new string[] { "If", "When" } },
            { KeywordType.IfStartSecond, new string[] { "then" } },
            { KeywordType.IfEnd, new string[] { "That's what I would do" } },
            { KeywordType.IfElse, new string[] { "Otherwise", "Or else" } },
            { KeywordType.SwitchDeclaration, new string[] { "In regards to" } },
            { KeywordType.SwitchCaseFirst, new string[] { "On the" } },
            { KeywordType.SwitchCaseSecond, new string[] { "hoof" } },
            { KeywordType.SwitchDefaultCase, new string[] { "If all else fails" } },
            { KeywordType.CycleEnding, new string[] { "That's what I did" } },
            { KeywordType.WhileStart, new string[] { "Here's what I did while", "As long as" } },
            { KeywordType.DoWhileStart, new string[] { "Here's what I did" } },
            { KeywordType.DoWhileEnd, new string[] { "I did this while", "I did this as long as" } },
            { KeywordType.ForStartFirst, new string[] { "For every" } },
            { KeywordType.ForStartSecond, new string[] { "from" } },
            { KeywordType.ForStartThird, new string[] { "to" } },
            { KeywordType.ForeachStartFirst, new string[] { "For every" } },
            { KeywordType.ForeachStartSecond, new string[] { "in" } },
        };

        private static Dictionary<KeywordType, string[]> variableTypesDictionary = new Dictionary<KeywordType, string[]>()
        {
            { KeywordType.Int, new string[] { "the number", "a number", "number", "numbers" } },
            { KeywordType.Char, new string[] { "character", "a character", "the character", "letter", "a letter", "the letter" } },
            { KeywordType.String, new string[] { "word", "phrase", "sentence", "quote", "name", "a word", "a phrase", "a sentence", "a quote", "a name",
                "the word", "the phrase", "the sentence", "the quote", "the name" } },
            { KeywordType.Bool, new string[] { "logic", "argument", "a logic", "an argument", "the logic", "the argument" } },

            { KeywordType.IntArray, new string[] { "numbers", "many numbers" } },
            { KeywordType.CharArray, new string[] { "letters",  "many letters", "characters", "many characters" } },
            { KeywordType.StringArray, new string[] { "many words", "many phrases", "many sentences", "many quotes", "many names" } },
            { KeywordType.BoolArray, new string[] { "logics", "arguments", "many arguments" } },
        };

        private static List<string> keywords = null;
        private static List<string> variableTypes = null;

        public static bool IsKeyword(KeywordType type, string keyword)
        {
            foreach (var cur in keywordsDictionary[type])
                if (cur == keyword)
                    return true;
            return false;
        }

        public static List<string> GetKeywords()
        {
            if (keywords != null) return keywords;
            keywords = new List<string>();
            foreach(var cur in keywordsDictionary)
                keywords.AddRange(cur.Value);
            return keywords;
        }

        public static List<string> GetVariableTypes()
        {
            if (variableTypes != null) return variableTypes;
            variableTypes = new List<string>();
            foreach (var cur in variableTypesDictionary)
                variableTypes.AddRange(cur.Value);
            return variableTypes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiM_Compiler.CodeGeneration.GenerationData
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public string VariableTypeValue
        {
            get
            {
                try
                {
                    return KeywordsDictionary.GetVariableType(Value);
                }
                catch(Exception)
                {
                    return Value;
                }
            }
        }

        public string ValueWithoutWhitespaces 
        { 
            get
            {
                return Value.Replace(" ", "");
            }
        }
        public List<Token> Childs { get; set; }

        public Token(TokenType type, string value = "", List<Token> childs = null)
        {
            Type = type;
            Value = value;
            Childs = childs;
        }

        public override string ToString()
        {
            string value = Value;
            if (string.IsNullOrWhiteSpace(value))
                value = $"Whitespace: length - {Value.Length}";
            else
                value = value.Replace("\n", "\\n");
            return $"Type - {Type.ToString()}; Value - {value};";
        }
    }
}

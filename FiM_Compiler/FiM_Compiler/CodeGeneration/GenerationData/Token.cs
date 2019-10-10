﻿using System;
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
        public List<Token> Childs { get; set; }
        //TODO mb should add childs as tokens???

        public Token(TokenType type, string value = "", List<Token> childs = null)
        {
            Type = type;
            Value = value;
            Childs = childs;
        }

        public override string ToString()
        {
            string value = Value;
            if (value == "\n")
                value = "\\n";
            else if (string.IsNullOrWhiteSpace(value))
                value = $"Whitespace: length - {Value.Length}";
            return $"Type - {Type.ToString()}; Value - {value};";
        }
    }
}
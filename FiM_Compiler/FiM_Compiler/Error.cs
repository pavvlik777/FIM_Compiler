using System;

namespace FiM_Compiler
{
    public class Error
    {
        public string ErrorText { get; set; }
        public bool IsWarning { get; set; }
        public int Line { get; set; }

        /// <summary>
        /// Creates error with message
        /// </summary>
        /// <param name="errorText">Error message</param>
        public Error(string errorText)
        {
            ErrorText = errorText;
            IsWarning = false;
            Line = -1;
        }

        /// <summary>
        /// Creates error on specific line with message
        /// </summary>
        /// <param name="errorText">Error message</param>
        /// <param name="line">Line of error</param>
        public Error(string errorText, int line)
        {
            ErrorText = errorText;
            IsWarning = false;
            Line = line;
        }

        /// <summary>
        /// Creates error or warning with message
        /// </summary>
        /// <param name="errorText">Error message</param>
        /// <param name="isWarning">Is warning</param>
        public Error(string errorText, bool isWarning)
        {
            ErrorText = errorText;
            IsWarning = isWarning;
            Line = -1;
        }

        public override string ToString()
        {
            string type = IsWarning ? "Warning" : "Error";
            string message = $"{type}: {ErrorText}";
            message += Line != -1 ? $" Line {Line}" : "";
            return $"{type}: {ErrorText}";
        }
    }
}
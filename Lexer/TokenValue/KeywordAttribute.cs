using System;

namespace Lexer.TokenValue
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class KeywordAttribute : Attribute
    {
        public string Value { get; set; }

        public KeywordAttribute(string content)
        {
            Value = content;
        }
    }   
}
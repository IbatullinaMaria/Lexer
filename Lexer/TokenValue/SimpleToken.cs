using System;

namespace Lexer.TokenValue
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class SimpleTokenAttribute : Attribute
    {
        public string Value { get; set; }

        public SimpleTokenAttribute(string content)
        {
            Value = content;
        }
    }
}
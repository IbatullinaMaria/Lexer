using System.Runtime.InteropServices;

namespace Lexer.TokenValue
{
    public class Global
    {
        public static int multyCommentFlag = 0;
        public static int stringFlag = 0;
        public static int arrayFlag = 0;
        public static char whiteSpace = ' ';
        public static char separator = '_';

        public static char[] byteDigit =
        {
            '0',
            '1'
        };

        public static char[] mainLimiters =
        {
            ',',
            '.',
            ';'
        };
        
        public static char[] limiters = 
        {
            '[',
            ']',
            '#',
            '$',
            '@',
            '{',
            '}',
            '(',
            ')',
            '>',
            '<',
            '+',
            '-',
            '=',
            '!',
            '*',
            '/',
            '%'
        };
        
        public static string[] reservedWords = 
        {
            "var",
            "Int",
            "Double",
            "Boolean",
            "String",
            "null",
            "if",
            "else",
            "do",
            "while",
            "for"
        };

        public static string[] boolValue =
        {
            "true",
            "false",
        };
    }
}
namespace Lexer.TokenValue
{
    public enum NumericSystem
    {
        [Keyword("x")] Hexadecimal,
        [Keyword("b")] Byte,
        [Keyword("o")] Octal,
        [Keyword("E")] WithExponent
    }

    public enum Keywords
    {
        Identificator,
        [Keyword("var")] Var,
        [Keyword("Int")] Int,
        [Keyword("Double")] Double,
        [Keyword("Boolean")] Boolean,
        [Keyword("String")] String,
        [Keyword("null")] Null,
        [Keyword("if")] If,
        [Keyword("else")] Else,
        [Keyword("do")] Do,
        [Keyword("while")] While,
        [Keyword("for")] For
    }
    
    public enum MainLimiters
    {
        [SimpleToken(".")] Dot,
        [SimpleToken(",")] Comma,
        [SimpleToken(";")] Semicolon
    }

    public enum Limiters
    {
        [SimpleToken("[")] LeftBracket,
        [SimpleToken("]")] RightBracket,

        [SimpleToken("#")] CommentOneLineLimiter,
        [SimpleToken("$")] CommentMultyLineLimiter,
        [SimpleToken("@")] StringLimiter,
        [SimpleToken("&")] ArrayLimiter,

        [SimpleToken("{")] LeftBrace,
        [SimpleToken("}")] RightBrace,

        [SimpleToken("(")] LeftParenthesis,
        [SimpleToken(")")] RightParenthesis,
        
        [SimpleToken(">")] Greater,
        [SimpleToken("<")] Less,
        [SimpleToken("+")] Plus,
        [SimpleToken("-")] Minus,
        [SimpleToken("=")] Assignment,
        [SimpleToken("!")] Not,
        [SimpleToken("*")] Multiply,
        [SimpleToken("/")] Divide,
        [SimpleToken("%")] Mod,
        
        [SimpleToken(">=")] GreaterOrEqual,
        [SimpleToken("<=")] LessOrEqual,
        [SimpleToken("==")] Equal,
        [SimpleToken("!=")] NotEqual
    }
    
}
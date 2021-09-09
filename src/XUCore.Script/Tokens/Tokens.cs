using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{

    /// <summary>
    /// List of all tokens.
    /// </summary>
    class Tokens
    {
        // Reserved keywords.
        internal readonly static Token Var          = new KeywordToken("var");
        internal readonly static Token If           = new KeywordToken("if");
        internal readonly static Token Else         = new KeywordToken("else");
        internal readonly static Token Break        = new KeywordToken("break");
        internal readonly static Token Continue     = new KeywordToken("continue");
        internal readonly static Token For          = new KeywordToken("for");
        internal readonly static Token While        = new KeywordToken("while");
        internal readonly static Token Function     = new KeywordToken("function");
        internal readonly static Token Return       = new KeywordToken("return");
        internal readonly static Token New          = new KeywordToken("new");
        internal readonly static Token Try          = new KeywordToken("try");
        internal readonly static Token Catch        = new KeywordToken("catch");
        internal readonly static Token Throw        = new KeywordToken("throw");
        internal readonly static Token In           = new KeywordToken("in");
        internal readonly static Token Run          = new KeywordToken("run");

        // Reserved literals.
        internal readonly static Token True         = new LiteralToken("true", true, true);
        internal readonly static Token False        = new LiteralToken("false", false, true);
        internal readonly static Token Null         = new LiteralToken("null", null, true);
        internal readonly static Token WhiteSpace   = new LiteralToken("''", string.Empty, true);
        internal readonly static Token NewLine      = new LiteralToken("newline", string.Empty, true);
        internal readonly static Token CommentSLine = new LiteralToken("comment_sl", string.Empty, true);
        internal readonly static Token CommentMLine = new LiteralToken("comment_ml", string.Empty, true);


        // Binary Math ( + - * / % )
        internal readonly static Token Plus              = new SymbolToken("+");
        internal readonly static Token Minus             = new SymbolToken("-");
        internal readonly static Token Multiply          = new SymbolToken("*");
        internal readonly static Token Divide            = new SymbolToken("/");
        internal readonly static Token Modulo            = new SymbolToken("%");

        // Comparision operators ( < <= > >= == != )
        internal readonly static Token LessThan          = new SymbolToken("<");
        internal readonly static Token LessThanOrEqual   = new SymbolToken("<=");
        internal readonly static Token MoreThan          = new SymbolToken(">");
        internal readonly static Token MoreThanOrEqual   = new SymbolToken(">=");
        internal readonly static Token EqualEqual        = new SymbolToken("==");
        internal readonly static Token NotEqual          = new SymbolToken("!=");
        internal readonly static Token LogicalAnd        = new SymbolToken("&&");
        internal readonly static Token LogicalOr         = new SymbolToken("||");
        internal readonly static Token LogicalNot        = new SymbolToken("!");
        internal readonly static Token Conditional       = new SymbolToken("?");
        internal readonly static Token Increment         = new SymbolToken("++");
        internal readonly static Token Decrement         = new SymbolToken("--");
        internal readonly static Token IncrementAdd      = new SymbolToken("+=");
        internal readonly static Token IncrementSubtract = new SymbolToken("-=");
        internal readonly static Token IncrementMultiply = new SymbolToken("*=");
        internal readonly static Token IncrementDivide   = new SymbolToken("/=");        
        internal readonly static Token LeftBrace         = new SymbolToken("{");
        internal readonly static Token RightBrace        = new SymbolToken("}");
        internal readonly static Token LeftParenthesis   = new SymbolToken("(");
        internal readonly static Token RightParenthesis  = new SymbolToken(")");
        internal readonly static Token LeftBracket       = new SymbolToken("[");
        internal readonly static Token RightBracket      = new SymbolToken("]");
        internal readonly static Token Semicolon         = new SymbolToken(";");
        internal readonly static Token Comma             = new SymbolToken(",");
        internal readonly static Token Dot               = new SymbolToken(".");
        internal readonly static Token Colon             = new SymbolToken(":");
        internal readonly static Token Assignment        = new SymbolToken("=");


        internal static IDictionary<string, Token> AllTokens = new Dictionary<string, Token>()
        {
            { "*",       Tokens.Multiply },
            { "/",       Tokens.Divide },
            { "+",       Tokens.Plus },
            { "-",       Tokens.Minus },
            { "%",       Tokens.Modulo },
            { "<",       Tokens.LessThan },
            { "<=",      Tokens.LessThanOrEqual },
            { ">",       Tokens.MoreThan },
            { ">=",      Tokens.MoreThanOrEqual },
            { "!=",      Tokens.NotEqual },
            { "==",      Tokens.EqualEqual },
            { "=",       Tokens.Assignment },
            { "!",       Tokens.LogicalNot },
            { ",",       Tokens.Comma },
            { ";",       Tokens.Semicolon },
            { ".",       Tokens.Dot },
            { "&&",      Tokens.LogicalAnd },
            { "||",      Tokens.LogicalOr },
            { "(",       Tokens.LeftParenthesis },
            { ")",       Tokens.RightParenthesis },
            { "{",       Tokens.LeftBrace },
            { "}",       Tokens.RightBrace },
            { "++",      Tokens.Increment },
            { "--",      Tokens.Decrement },
            { "*=",      Tokens.IncrementMultiply },
            { "/=",      Tokens.IncrementDivide },
            { "+=",      Tokens.IncrementAdd },
            { "-=",      Tokens.IncrementSubtract },

            { "true",    Tokens.True },
            { "false",   Tokens.False },
            { "null",    Tokens.Null },
            { " ",       Tokens.WhiteSpace },

            { "var",     Tokens.Var },
            { "if",      Tokens.If }, 
            { "else",    Tokens.Else }, 
            { "for",     Tokens.For },
            { "while",   Tokens.While },
            { "function",Tokens.Function },
            { "break",   Tokens.Break },
            { "continue",Tokens.Continue },
            { "new",     Tokens.New },
            { "return",  Tokens.Return },
            { "try",     Tokens.Try },
            { "catch",   Tokens.Catch },
            { "throw",   Tokens.Throw },
            { "in",      Tokens.In },
            { "run",     Tokens.Run }
        };


        internal static IDictionary<string, Token> CompareTokens = new Dictionary<string, Token>()
        {
            { "<",       Tokens.LessThan },
            { "<=",      Tokens.LessThanOrEqual },
            { ">",       Tokens.MoreThan },
            { ">=",      Tokens.MoreThanOrEqual },
            { "!=",      Tokens.NotEqual },
            { "==",      Tokens.EqualEqual }          
        };


        internal static IDictionary<string, Token> MathTokens = new Dictionary<string, Token>()
        {
            { "*",       Tokens.Multiply },
            { "/",       Tokens.Divide },
            { "+",       Tokens.Plus },
            { "-",       Tokens.Minus },
            { "%",       Tokens.Modulo }         
        };


        internal static IDictionary<string, Operator> AllOps = new Dictionary<string, Operator>()
        {
            { "*", Operator.Multiply },
            { "/", Operator.Divide }, 
            { "+", Operator.Add },
            { "-", Operator.Subtract },
            { "%", Operator.Modulus },
            { "<", Operator.LessThan },
            { "<=", Operator.LessThanEqual },
            { ">", Operator.MoreThan },
            { ">=", Operator.MoreThanEqual },
            { "==", Operator.EqualEqual },
            { "!=", Operator.NotEqual },
            { "=", Operator.Equal },
            { "&&", Operator.And },
            { "||", Operator.Or },
            { "(", Operator.LeftParenthesis },
            { ")", Operator.RightParenthesis },
            { "{", Operator.LeftBrace },
            { "}", Operator.RightBrace },
            { "[", Operator.LeftBracket },
            { "]", Operator.RightBracket },
            { "++", Operator.PlusPlus },
            { "--", Operator.MinusMinus },
            { "+=", Operator.PlusEqual },
            { "-=", Operator.MinusEqual },
            { "*=", Operator.MultEqual },
            { "/=", Operator.DivEqual },
            { ",",  Operator.Comma },
            { "!", Operator.LogicalNot },
            { ".", Operator.Dot }
        };


        internal static IDictionary<string, int> OpsPrecedence = new Dictionary<string, int>()
        {
            { "(",       9 },
            { ")",       9 },
            { "*",       7 },
            { "/",       7 },
            { "%",       7 },
            { "+",       6 },
            { "-",       6 },
            { "<",       4 },
            { "<=",      4 },
            { ">",       4 },
            { ">=",      4 },
            { "!=",      1 },
            { "==",      1 },
            { "&&",      0 },
            { "||",      0 }
        };


        internal static IDictionary<Operator, bool> MathOps = new Dictionary<Operator, bool>()
        {
            { Operator.Multiply, true },
            { Operator.Divide, true },
            { Operator.Add, true },
            { Operator.Subtract, true },
            { Operator.Modulus, true },
        };


        internal static IDictionary<Operator, bool> CompareOps = new Dictionary<Operator, bool>()
        {
            { Operator.LessThan, true },
            { Operator.LessThanEqual, true },
            { Operator.MoreThan, true },
            { Operator.MoreThanEqual, true },
            { Operator.EqualEqual, true },
            { Operator.NotEqual, true },
        };


        internal static IDictionary<Operator, bool> IncrementOps = new Dictionary<Operator, bool>()
        {
            { Operator.PlusPlus, true },
            { Operator.PlusEqual, true },
            { Operator.MinusMinus, true },
            { Operator.MinusEqual, true },
            { Operator.MultEqual, true },
            { Operator.DivEqual, true },
        };


        /// <summary>
        /// Determines if the text supplied is a literal token
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsLiteral(string key)
        {
            return IsTokenType<LiteralToken>(key);
        }


        /// <summary>
        /// Get the literal token from the string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static LiteralToken ToLiteral(string key)
        {
            return AllTokens[key] as LiteralToken;
        }


        /// <summary>
        /// Get the keyword token from the string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static SymbolToken ToOperator(string key)
        {
            return AllTokens[key] as SymbolToken;
        }


        /// <summary>
        /// Get the operator as an enum
        /// </summary>
        /// <returns></returns>
        public static Operator ToOp(string op)
        {
            return AllOps[op];
        }


        /// <summary>
        /// Gets whether or not this is a keyword
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsKeyword(string key)
        {
            return IsTokenType<KeywordToken>(key);
        }


        /// <summary>
        /// Get the keyword token from the string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static KeywordToken ToKeyWord(string key)
        {
            return AllTokens[key] as KeywordToken;
        }        


        /// <summary>
        /// Determines if the text supplied is a literal token
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsTokenType<T>(string key)
        {
            bool contains = AllTokens.ContainsKey(key);
            if (!contains) return false;
            Token t = AllTokens[key];
            return t is T;
        }


        /// <summary>
        /// Checks if the token supplied is a math op ( * / + - % )
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsMath(Token token)
        {
            return MathTokens.ContainsKey(token.Text);
        }


        /// <summary>
        /// Checks if the operator supplied is a binary op ( * / + - % )
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool IsMath(Operator op)
        {
            return MathOps.ContainsKey(op);
        }


        /// <summary>
        /// Whether or not this following text is an operator that has precedence value.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool IsOp(string op)
        {
            return OpsPrecedence.ContainsKey(op);
        }


        /// <summary>
        /// Checks if the operator is a conditional 
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool IsConditional(Operator op)
        {
            return op == Operator.And || op == Operator.Or;
        }


        /// <summary>
        /// Checks if the token is a comparison token ( less lessthan more morethan equal not equal ).
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool IsCompare(Token token)
        {
            return CompareTokens.ContainsKey(token.Text);
        }


        /// <summary>
        /// Checks if the operator is a comparison operator ( less lessthan more morethan equal not equal ).
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool IsCompare(Operator op)
        {
            return CompareOps.ContainsKey(op);
        }


        /// <summary>
        /// Checks if the operator supplied is a binary op ( * / + - % )
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool IsIncrement(Operator op)
        {
            return IncrementOps.ContainsKey(op);
        }


        /// <summary>
        /// Checks if the operator supplied is a binary op ( * / + - % )
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static bool IsLogical(Operator op)
        {
            return op == Operator.And || op == Operator.Or;
        }


        /// <summary>
        /// Gets the operator precendence.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int Precedence(string token)
        {
            int precedence = OpsPrecedence[token];
            return precedence;
        }
    }
}

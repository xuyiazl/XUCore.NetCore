using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Base class for the parser
    /// </summary>
    public class ParserBase
    {
        #region Protected members
        /// <summary>
        /// Scope of the script
        /// </summary>
        protected Scope _scope = null;


        /// <summary>
        /// Context information about the script.
        /// </summary>
        protected Context _context = null;


        /// <summary>
        /// Scanner to parse characters
        /// </summary>
        protected Scanner _scanner = null;


        /// <summary>
        /// Lexer to parse tokens.
        /// </summary>
        protected Lexer _lexer = null;


        /// <summary>
        /// The script as text.
        /// </summary>
        protected string _script;


        /// <summary>
        /// The path to the script if script was provided as a file path instead of text
        /// </summary>
        protected string _scriptPath;


        /// <summary>
        /// The parsed statements from interpreting the tokens.
        /// </summary>
        protected List<Statement> _statements; 


        /// <summary>
        /// The state of the parser .. used in certain cases.
        /// </summary>
        protected ParserState _state;


        /// <summary>
        /// Settings of the lanaguage interpreter.
        /// </summary>
        protected LangSettings _settings;


        /// <summary>
        /// Token iterator.
        /// </summary>
        protected TokenIterator _tokenIt;
        #endregion


        /// <summary>
        /// Initialize
        /// </summary>
        public ParserBase()
        {
            _scanner = new Scanner();
            _lexer = new Lexer(_scanner);
        }


        #region Public properties
        /// <summary>
        /// Get the scope
        /// </summary>
        public Scope Scope { get { return _scope; } }


        /// <summary>
        /// Get/Set the context of the script.
        /// </summary>
        public Context Context { get { return _context; } set { _context = value; _scope = _context.Scope; } }


        /// <summary>
        /// Name of the current script being parsed.
        /// Set from the Interpreter object.
        /// </summary>
        public string ScriptName { get; set; }


        /// <summary>
        /// Get the lexer.
        /// </summary>
        public Lexer Lexer { get { return _lexer; } }


        /// <summary>
        /// Settings
        /// </summary>
        public LangSettings Settings { get { return _settings; } set { _settings = value; } }
        #endregion


        #region Public methods
        /// <summary>
        /// Intialize.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="scope"></param>
        protected virtual void Init(string script, Scope scope)
        {
            _script = script;
            _scriptPath = string.Empty;
            _statements = new List<Statement>();
            _scope = scope == null ? new Scope() : scope;
            _scanner.Init(_script);
            _state = new ParserState();
        }
        #endregion


        #region Expect calls
        /// <summary>
        /// Advance to the next token and expect the token supplied.
        /// </summary>
        /// <param name="expectedToken"></param>
        protected void AdvanceAndExpect(Token expectedToken)
        {
            _tokenIt.Advance();
            Expect(expectedToken);
        }


        /// <summary>
        /// Expect the token supplied and advance to next token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="advance">Whether or not to advance to the next token after expecting token</param>
        protected void Expect(Token token, bool advance = true)
        {
            if (_tokenIt.NextToken.Token == Token.EndToken) throw BuildEndOfScriptException();
            if (_tokenIt.NextToken.Token != token) throw BuildSyntaxExpectedTokenException(token.Text);
            if (advance) _tokenIt.Advance();
        }


        /// <summary>
        /// Expect the token supplied and advance to next token
        /// </summary>
        /// <param name="token1">The 1st token to expect</param>
        /// <param name="token2">The 2nd token to expect</param>
        /// <param name="token3">The 3rd token to expect</param>
        /// <param name="advance">Whether or not to advance to the next token after expecting token</param>
        protected void ExpectMany(Token token1, Token token2 = null, Token token3 = null, bool advance = true)
        {
            if (_tokenIt.NextToken.Token == Token.EndToken) throw BuildEndOfScriptException(); 

            // ASSERT token 1 match
            if (_tokenIt.NextToken.Token != token1) throw BuildSyntaxExpectedTokenException(token1.Text);

            // No token 2 ?
            if (token2 == null) { if (advance) _tokenIt.Advance(); return; }
            
            // Advance to token 2
            _tokenIt.Advance();

            // ASSERT token 2 match
            if (_tokenIt.NextToken.Token != token2) throw BuildSyntaxExpectedTokenException(token2.Text);

            // No token 3
            if (token3 == null) { if (advance) _tokenIt.Advance(); return; }

            // Advance to token 3.
            _tokenIt.Advance();

            // ASSERT token 3 match
            if (_tokenIt.NextToken.Token != token3) throw BuildSyntaxExpectedTokenException(token3.Text);

            if (advance) _tokenIt.Advance();
        }


        /// <summary>
        /// Expect identifier
        /// </summary>
        /// <returns></returns>
        protected string ExpectId(bool advance = true, bool allowLiteralAsId = false)
        {
            if (_tokenIt.NextToken.Token == Token.EndToken) throw BuildEndOfScriptException();
            if (!allowLiteralAsId && !(_tokenIt.NextToken.Token is IdToken)) throw BuildSyntaxExpectedTokenException("identifier");

            string id = _tokenIt.NextToken.Token.Text;

            if (advance)
                _tokenIt.Advance();

            return id;
        }
        #endregion


        #region Helpers
        /// <summary>
        /// Convert the script into a series of tokens.
        /// </summary>
        protected void Tokenize()
        {
            var tokens = _lexer.Tokenize();
            _tokenIt = new TokenIterator();
            _tokenIt.Init(tokens, _lexer.LineNumber, _lexer.LineCharPos);
        }


        /// <summary>
        /// Build a language exception due to the current token being invalid.
        /// </summary>
        /// <param name="expected"></param>
        /// <returns></returns>
        protected LangException BuildSyntaxExpectedTokenException(string expected)
        {
            return new LangException("Syntax Error", string.Format("Expected {0} but found '{1}'", expected, _tokenIt.NextToken.Token.Text), _scriptPath, _tokenIt.NextToken.Line, _tokenIt.NextToken.CharPos);
        }


        /// <summary>
        /// Build a language exception due to the current token being invalid.
        /// </summary>
        /// <returns></returns>
        protected LangException BuildSyntaxUnexpectedTokenException()
        {
            return new LangException("Syntax Error", string.Format("Unexpected token found '{0}'", _tokenIt.NextToken.Token.Text), _scriptPath, _tokenIt.NextToken.Line, _tokenIt.NextToken.CharPos);
        }

        /// <summary>
        /// Builds a language exception due to a specific limit being reached.
        /// </summary>
        /// <param name="error">Error message</param>
        /// <param name="limittype">FuncParameters</param>
        /// <param name="limit">Limit number</param>
        /// <returns></returns>
        protected LangException BuildLimitException(string error, int limit, string limittype = "")
        {
            return new LangException("Limit Error", error, _scriptPath, _tokenIt.NextToken.Line, _tokenIt.NextToken.CharPos);
        }


        /// <summary>
        /// Builds a language exception due to the unexpected end of script.
        /// </summary>
        /// <returns></returns>
        protected LangException BuildEndOfScriptException()
        {
            return new LangException("Syntax Error", "Unexpected end of script", _scriptPath, _tokenIt.NextToken.Line, _tokenIt.NextToken.CharPos);
        }


        /// <summary>
        /// End of statement script.
        /// </summary>
        /// <param name="endOfStatementToken"></param>
        /// <returns></returns>
        protected bool IsEndOfStatementOrEndOfScript(Token endOfStatementToken)
        {
            return IsEndOfStatement(endOfStatementToken) || IsEndOfScript();
        }


        /// <summary>
        /// Whether at end of statement.
        /// </summary>
        /// <returns></returns>
        protected bool IsEndOfStatement(Token endOfStatementToken)
        {
            return (_tokenIt.NextToken.Token == endOfStatementToken);
        }


        /// <summary>
        /// Whether at end of script
        /// </summary>
        /// <returns></returns>
        protected bool IsEndOfScript()
        {
            return _tokenIt.NextToken.Token == Token.EndToken;
        }


        /// <summary>
        /// Parses a sequence of names/identifiers.
        /// </summary>
        /// <returns></returns>
        protected List<string> ParseNames()
        {
            var names = new List<string>();
            while (true)
            {
                // Case 1: () empty list
                if (IsEndOfStatementOrEndOfScript(Tokens.RightParenthesis))
                    break;

                // Case 2: name and auto-advance to next token
                var name = ExpectId(true);
                names.Add(name);

                // Case 3: only 1 argument. 
                if (IsEndOfStatementOrEndOfScript(Tokens.RightParenthesis))
                    break;

                // Case 4: comma, more names to come
                if (_tokenIt.NextToken.Token == Tokens.Comma) _tokenIt.Advance();
            }
            return names;
        }


        /// <summary>
        /// Sets the script position of the node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="token"></param>
        protected void SetScriptPosition(AstNode node, TokenData token = null)
        {
            if (token == null) token = _tokenIt.NextToken;
            node.Ref = new ScriptRef(ScriptName, token.Line, token.CharPos);
        }
        #endregion
    }
}

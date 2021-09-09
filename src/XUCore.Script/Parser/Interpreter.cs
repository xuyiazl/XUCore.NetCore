using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace XUCore.Script
{
    /// <summary>
    /// Light version of javascript with some "sandbox" features coming up.
    /// </summary>
    /// <remarks>
    /// Provides high-level functionality for parsing/executing scripts.
    /// 
    /// Features include:
    /// 1. Convert script into a list of tokens ( Using Lexer ) - prints out line numbers and char positions of each token
    /// 2. Convert script into a sequence of expressions/statements (Using Parser ) - prints out line numbers and char positions of exp/stmts.
    /// 3. Only parse without executing
    /// 4. Parse and execute.
    /// 5. Provides benchmark capabilities of executing each statement.
    /// </remarks>
    public class Interpreter
    {
        //private InterpreterSettings _settings;
        private Scope _scope;
        private Parser _parser;
        private Context _context;
        private LangSettings _settings;
        private RunResult _runResult;


        /// <summary>
        /// Initialize
        /// </summary>
        public Interpreter()
        {
            _settings = new LangSettings();
            _context = new Context();
            _scope = _context.Scope;
            _parser = new Parser();
            _parser.Context = _context;
            _parser.Context.Settings = _settings;
            _parser.Context.Limits.Init();
            _parser.Settings = _settings;
            InitSystemFunctions();
        }


        /// <summary>
        /// Scope of the script
        /// </summary>
        public Scope Scope
        {
            get { return _context.Scope; }
        }


        /// <summary>
        /// Context for the script.
        /// </summary>
        public Context Context
        {
            get { return _context; }
        }


        /// <summary>
        /// Run result
        /// </summary>
        public RunResult Result
        {
            get { return _runResult; }
        }
        

        /// <summary>
        /// Register the callback for custom functions
        /// </summary>
        /// <param name="funcCallPattern">Pattern for the function e.g. "CreateUser", or "Blog.*"</param>
        /// <param name="callback">The callback to call</param>
        public void SetFunctionCallback(string funcCallPattern, Func<FunctionCallExpression, object> callback)
        {
            _parser.Context.ExternalFunctions.Register(funcCallPattern, callback);
        }


        /// <summary>
        /// Parses the script but does not execute it.
        /// </summary>
        /// <param name="scriptPath">Path to the script</param>
        public void ParseFile(string scriptPath)
        {
            Execute(() =>
            {
                var script = ReadFile(scriptPath);
                Parse(script);
            });
        }


        /// <summary>
        /// Parses the script but does not execute it.
        /// </summary>
        /// <param name="script"></param>
        public void Parse(string script)
        {
            Execute(() =>
            {
                _context.Limits.CheckScriptLength(script);
                _parser.Parse(script, _scope);
            });
        }


        /// <summary>
        /// Executes the file.
        /// </summary>
        /// <param name="scriptPath">Path to the script</param>
        public void ExecuteFile(string scriptPath)
        {
            Execute(() =>
            {
                var script = ReadFile(scriptPath);
                Execute(script);
            });
        }


        /// <summary>
        /// Executes the script
        /// </summary>
        /// <param name="script">Script text</param>
        public void Execute(string script)
        {
            Execute(() =>
            {
                _context.Limits.CheckScriptLength(script);
                _parser.Parse(script, _scope);
                _parser.Execute();
            });
        }


        /// <summary>
        /// Replaces a token with another token.
        /// </summary>
        /// <param name="text">The text to replace</param>
        /// <param name="newValue">The replacement text</param>
        public void LexReplace(string text, string newValue)
        {
            _parser.Lexer.SetReplacement(text, newValue);
        }


        /// <summary>
        /// Removes a token during the lexing process.
        /// </summary>
        /// <param name="text">The text to remove</param>
        public void LexRemove(string text)
        {
            _parser.Lexer.SetRemoval(text);
        }


        /// <summary>
        /// Adds a token during the lexing process.
        /// </summary>
        /// <param name="before">whether to insert before or after</param>
        /// <param name="text">The text to check for inserting before/after</param>
        /// <param name="newValue">The new value to insert before/after</param>
        public void LexInsert(bool before, string text, string newValue)
        {
            _parser.Lexer.SetInsert(before, text, newValue);
        }


        /// <summary>
        /// Convert the script to a series of tokens.
        /// </summary>
        /// <param name="script">The script content or file name</param>
        /// <param name="isFile">Whether or not the script supplied is a filename or actual script content</param>
        /// <returns></returns>
        public List<TokenData> ToTokens(string script, bool isFile)
        {
            List<TokenData> tokens = null;
            if (isFile)
            {
                script = File.ReadAllText(script);
            }
            var scanner = new Scanner(script);
            var lexer = new Lexer(scanner);

            Execute(() =>
            {
                tokens = lexer.Tokenize();
            },
            () => string.Format("Last token: {0}, Line : {1}, Pos : {2} ", lexer.LastToken.Text, lexer.LineNumber, lexer.LineCharPos));
            return tokens;
        }


        /// <summary>
        /// Convert the script to a series of tokens.
        /// </summary>
        /// <param name="script">The script content or file name</param>
        /// <param name="isFile">Whether or not the script supplied is a filename or actual script content</param>
        /// <returns></returns>
        public List<Statement> ToStatements(string script, bool isFile)
        {
            List<Statement> statements = null;
            Execute(() =>
            {
                statements = _parser.Parse(script);
            });
            return statements;            
        }


        /// <summary>
        /// Prints tokens to file supplied, if file is not supplied, prints to console.
        /// </summary>
        /// <param name="scriptFile">The source script file</param>
        /// <param name="toFile">The file to write the token info to.</param>
        public void PrintTokens(string scriptFile, string toFile)
        {
            var tokens = ToTokens(scriptFile, true);
            using (StreamWriter writer = new StreamWriter(toFile))
            {
                foreach (TokenData tokendata in tokens)
                {
                    writer.WriteLine(tokendata.ToString());
                }
                writer.Flush();
            };
        }


        /// <summary>
        /// Prints tokens to file supplied, if file is not supplied, prints to console.
        /// </summary>
        /// <param name="scriptFile">The source script file</param>
        /// <param name="toFile">The file to write the statement info to.</param>
        public void PrintStatements(string scriptFile, string toFile)
        {
            var statements = ToStatements(scriptFile, true);
            using (StreamWriter writer = new StreamWriter(toFile))
            {
                foreach (Statement stmt in statements)
                {
                    writer.Write(stmt.AsString());
                }
                writer.Flush();
            };
        }


        /// <summary>
        /// Prints the run result to the file path specified.
        /// </summary>
        /// <param name="toFile"></param>
        public void PrintRunResult(string toFile)
        {
            using (StreamWriter writer = new StreamWriter(toFile))
            {
                writer.Write(_runResult.ToString());
                writer.Flush();
            };
        }


        #region Private methods
        private string ReadFile(string scriptPath)
        {
            if (!File.Exists(scriptPath))
                throw new FileNotFoundException(scriptPath);

            var script = File.ReadAllText(scriptPath);
            return script;
        }


        private void Execute(Action action, Func<string> exceptionMessageFetcher = null)
        {
            DateTime start = DateTime.Now;
            bool success = true;
            string message = string.Empty;  
            Exception scriptError = null;
            try
            {
                action();
            }
            catch (Exception ex)
            {
                success = false;
                if (ex is LangException)
                {
                    LangException lex = ex as LangException;
                    const string langerror = "{0} : {1} at line : {2}, position: {3}";
                    message = string.Format(langerror, lex.ErrorType, lex.Message, lex.LineNumber, lex.CharPostion);
                }
                else message = ex.Message;

                scriptError = ex;
                if (exceptionMessageFetcher != null)
                    message += exceptionMessageFetcher();
            }
            DateTime end = DateTime.Now;
            _runResult = new RunResult(start, end, success, message);
            _runResult.Ex = scriptError;
        }


        private void InitSystemFunctions()
        {
            // Print and log functions.
            _parser.Context.ExternalFunctions.Register("print", (exp) => FunctionHelper.Print(_settings, exp, false));
            _parser.Context.ExternalFunctions.Register("printl", (exp) => FunctionHelper.Print(_settings, exp, true));
            _parser.Context.ExternalFunctions.Register("log", (exp) => FunctionHelper.Log(_settings, exp));

        }
        #endregion
    }
}

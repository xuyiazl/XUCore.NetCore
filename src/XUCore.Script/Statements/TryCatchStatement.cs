using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;


namespace XUCore.Script
{
    /// <summary>
    /// For loop Expression data
    /// </summary>
    public class TryCatchStatement : BlockStatement
    {
        /// <summary>
        /// Create new instance
        /// </summary>
        public TryCatchStatement()
        {
            Catch = new BlockStatement();
        }


        /// <summary>
        /// Name for the error in the catch clause.
        /// </summary>
        public string ErrorName;


        /// <summary>
        /// Else statement.
        /// </summary>
        public BlockStatement Catch;



        /// <summary>
        /// Execute
        /// </summary>
        public override void Execute()
        {
            bool tryScopePopped = false;
            bool catchScopePopped = false;
            try
            {
                Ctx.Scope.Push();
                LangHelper.Execute(_statements, this);
                Ctx.Scope.Pop();
                tryScopePopped = true;
            }
            // Force the langlimit excpetion to propegate 
            // do not allow to flow through to the catch all "Exception ex".
            catch (LangLimitException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Ctx.Limits.CheckExceptions(this);

                // Pop the try scope.
                if (!tryScopePopped) Ctx.Scope.Pop();

                // Push the scope in the catch block
                Ctx.Scope.Push();
                Ctx.Scope.SetValue(ErrorName, LError.FromException(ex));

                // Run statements in catch block.
                if (Catch != null && Catch.Statements.Count > 0)
                    LangHelper.Execute(Catch.Statements, Catch);

                // Pop the catch scope.
                Ctx.Scope.Pop();
                catchScopePopped = true;
            }
            finally
            {
                // Pop the catch scope in case there was an error.
                if(!catchScopePopped) Ctx.Scope.Remove(ErrorName);
            }
        }
    }    
}

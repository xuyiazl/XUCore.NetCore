using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Context information for the script.
    /// </summary>
    public class Context
    {
        /// <summary>
        /// Creates new instance with default Functions/Types/Scope.
        /// </summary>
        public Context()
        {
            Types = new RegisteredTypes();
            ExternalFunctions = new ExternalFunctions();
            Functions = new RegisteredFunctions();
            Scope = new Scope();
            Limits = new Limits(this);
            CallStack stack = new CallStack(Limits.CheckCallStack);
            State = new LangState(stack);
        }


        /// <summary>
        /// Registered custom functions outside of script
        /// </summary>
        public ExternalFunctions ExternalFunctions;


        /// <summary>
        /// Script functions
        /// </summary>
        public RegisteredFunctions Functions;


        /// <summary>
        /// Registered custom types
        /// </summary>
        public RegisteredTypes Types;


        /// <summary>
        /// Scope of the script.
        /// </summary>
        public Scope Scope;


        /// <summary>
        /// Settings.
        /// </summary>
        public LangSettings Settings;


        /// <summary>
        /// State of the language.
        /// </summary>
        public LangState State;


        /// <summary>
        /// Limits for 
        /// </summary>
        internal Limits Limits;
    }
}

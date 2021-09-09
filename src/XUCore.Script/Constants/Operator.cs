using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XUCore.Script
{
    /// <summary>
    /// Token for the language.
    /// </summary>
    public enum Operator
    {
        /* BINARY + - * / %  */
        /// <summary>
        /// +
        /// </summary>
        Add,


        /// <summary>
        /// -
        /// </summary>
        Subtract,
        
        
        /// <summary>
        /// * 
        /// </summary>
        Multiply,


        /// <summary>
        /// /
        /// </summary>
        Divide,


        /// <summary>
        /// %
        /// </summary>
        Modulus,


        /* COMPARE < <= > >= != ==  */
        /// <summary>
        /// &lt;
        /// </summary>
        LessThan,


        /// <summary>
        /// &lt;=
        /// </summary>
        LessThanEqual,


        /// <summary>
        /// >
        /// </summary>
        MoreThan,


        /// <summary>
        /// >=
        /// </summary>
        MoreThanEqual,

                
        /// <summary>
        /// =
        /// </summary>
        Equal,


        /// <summary>
        /// ==
        /// </summary>
        EqualEqual,


        /// <summary>
        /// !=
        /// </summary>
        NotEqual,


        /* CONDITION && ||  */
        /// <summary>
        /// and
        /// </summary>
        And,

        /// <summary>
        /// ||
        /// </summary>
        Or,


        /* UNARY ++ -- += -= *= /= */
        /// <summary>
        /// ++
        /// </summary>
        PlusPlus,


        /// <summary>
        /// --
        /// </summary>
        MinusMinus,


        /// <summary>
        /// += 
        /// </summary>
        PlusEqual,


        /// <summary>
        /// -=
        /// </summary>
        MinusEqual,


        /// <summary>
        /// *=
        /// </summary>
        MultEqual,


        /// <summary>
        /// /=
        /// </summary>
        DivEqual,


        /// <summary>
        /// (
        /// </summary>
        LeftParenthesis,

                
        /// <summary>
        /// {
        /// </summary>
        LeftBrace,


        /// <summary>
        /// [
        /// </summary>
        LeftBracket,


        /// <summary>
        /// )
        /// </summary>
        RightParenthesis,

        
        /// <summary>
        /// ]
        /// </summary>
        RightBracket,


        /// <summary>
        /// }
        /// </summary>
        RightBrace,

        
        /// <summary>
        /// ,
        /// </summary>
        Comma,


        /// <summary>
        /// !
        /// </summary>
        LogicalNot,


        /// <summary>
        /// .
        /// </summary>
        Dot
    }


/*
    /// <summary>
    /// How operators are processed.
    /// </summary>
    enum Associativity
    {
        LeftToRight,
        RightToLeft
    }


    
    /// <summary>
    /// Information about an operator.
    /// </summary>
    public class Op
    {
        /// <summary>
        /// The underling operator.
        /// </summary>
        public readonly Operator Operator;


        /// <summary>
        /// The associativity of the operator.
        /// </summary>
        public readonly Associativity Assoc;

        
        /// <summary>
        /// Precedence of the operator.
        /// </summary>
        public readonly int Precedence;


        public Op(Operator op, Associativity assoc, int precedence)
        {
            this.Operator = op;
            this.Assoc = assoc;
            this.Precedence = precedence;
        }
    }
    */
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;


namespace XUCore.Script
{
    /// <summary>
    /// Variable expression data
    /// </summary>
    class AssignmentStatement : Statement
    {
        private bool _isDeclaration;
        private Expression VarExp;
        private Expression ValueExp;
        private List<Tuple<Expression, Expression>> _declarations;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="isDeclaration">Whether or not the variable is being declared in addition to assignment.</param>
        /// <param name="name">Name of the variable</param>
        /// <param name="valueExp">Expression representing the value to set variable to.</param>
        public AssignmentStatement(bool isDeclaration, string name, Expression valueExp)
            : this(isDeclaration, new VariableExpression(name), valueExp)
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="isDeclaration">Whether or not the variable is being declared in addition to assignment.</param>
        /// <param name="varExp">Expression representing the variable name to set</param>
        /// <param name="valueExp">Expression representing the value to set variable to.</param>
        public AssignmentStatement(bool isDeclaration, Expression varExp, Expression valueExp)
        {
            this._isDeclaration = isDeclaration;
            this._declarations = new List<Tuple<Expression, Expression>>();
            this._declarations.Add(new Tuple<Expression, Expression>(varExp, valueExp));
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="isDeclaration">Whether or not the variable is being declared in addition to assignment.</param>
        /// <param name="declarations"></param>        
        public AssignmentStatement(bool isDeclaration, List<Tuple<Expression, Expression>> declarations)
        {
            this._isDeclaration = isDeclaration;
            this._declarations = declarations;
        }
        

        /// <summary>
        /// Evaluate
        /// </summary>
        /// <returns></returns>
        public override void  Execute()
        {
            object result = null;
            foreach (var assigment in _declarations)
            {
                this.VarExp = assigment.Item1;
                this.ValueExp = assigment.Item2;
                                
                // CASE 1 & 2
                if (this.VarExp is VariableExpression)
                {
                    string varname = ((VariableExpression)this.VarExp).Name;

                    // Case 1: var result;
                    if (this.ValueExp == null)
                    {
                        this.Ctx.Scope.SetValue(varname, LNull.Instance, _isDeclaration);
                    }
                    else
                    {
                        // Case 2: var result = <expression>;
                        result = this.ValueExp.Evaluate();

                        // CHECK_LIMIT:
                        Ctx.Limits.CheckStringLength(this, result);

                        this.Ctx.Scope.SetValue(varname, result, _isDeclaration);
                    }

                    // LIMIT CHECK
                    Ctx.Limits.CheckScopeCount(this.VarExp);
                    Ctx.Limits.CheckScopeStringLength(this.VarExp);
                }
                // CASE 3 - 4 : Member access via class / map                    
                else if (this.VarExp is MemberAccessExpression)
                {
                    result = this.ValueExp.Evaluate();

                    // CHECK_LIMIT:
                    Ctx.Limits.CheckStringLength(this, result);

                    // Case 3: Set property "user.name" = <expression>;
                    var varResult = this.VarExp.Evaluate();
                    if (varResult is Tuple<object, string, string, PropertyInfo>)
                    {
                        var info = varResult as Tuple<object, string, string, PropertyInfo>;
                        info.Item4.SetValue(info.Item1, result, null);
                    }
                    // Case 4: Set map "user.name" = <expression>; // { name: 'kishore' }
                    else if (varResult is Tuple<LMap, string>)
                    {
                        var map = varResult as Tuple<LMap, string>;
                        map.Item1.SetValue(map.Item2, result);
                    }
                }
                // Case 5: Set index value "users[0]" = <expression>;
                else if (this.VarExp is IndexExpression)
                {
                    result = this.ValueExp.Evaluate();

                    // CHECK_LIMIT:
                    Ctx.Limits.CheckStringLength(this, result);

                    var indexExp = this.VarExp.Evaluate() as Tuple<object, int>;
                    var obj = indexExp.Item1;
                    var ndx = indexExp.Item2;
                    if (obj is Array)
                    {
                        obj.GetType().GetMethod("SetValue", new Type[] { typeof(int) }).Invoke(obj, new object[] { result, ndx });
                    }
                    else if (obj is LArray)
                    {
                        ((LArray)obj).SetByIndex(ndx, result);
                    }
                    else
                    {
                        obj.GetType().GetMethod("set_Item").Invoke(obj, new object[] { result, ndx });
                    }
                }
            }
        }
    }
}

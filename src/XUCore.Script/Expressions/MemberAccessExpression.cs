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
    /// Member access expressions for "." property or "." method.
    /// </summary>
    public class MemberAccessExpression : Expression
    {
        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="variableExp">The variable expression to use instead of passing in name of variable.</param>
        /// <param name="memberName">Name of member, this could be a property or a method name.</param>
        /// <param name="isAssignment">Whether or not this is part of an assigment</param>
        public MemberAccessExpression(Expression variableExp, string memberName, bool isAssignment)
        {
            this.VariableExp = variableExp;
            this.MemberName = memberName;
            this.IsAssignment = isAssignment;
        }

        /// <summary>
        /// The variable expression representing the list.
        /// </summary>
        public Expression VariableExp;


        /// <summary>
        /// The name of the member.
        /// </summary>
        public string MemberName;


        /// <summary>
        /// Whether or not this member access is part of an assignment.
        /// </summary>
        public bool IsAssignment;


        /// <summary>
        /// Either external function or member name.
        /// </summary>
        /// <returns></returns>
        public override object Evaluate()
        {
            Type type = null;
            string variableName = string.Empty;
            // Case 1: "user.create" -> external or internal script.
            if (VariableExp is VariableExpression )
            {
                var exp = VariableExp as VariableExpression;
                variableName = exp.Name;
                string funcname = variableName + "." + MemberName;

                // External function
                if(Ctx.ExternalFunctions.Contains(funcname))
                    return funcname;

                // Case "Person.Create" -> static method call on custom object?
                if (Ctx.Types.Contains(variableName))
                {
                    type = Ctx.Types.Get(variableName);
                    return new Tuple<string, string, MethodInfo>(type.Name, MemberName, type.GetMethod(MemberName));
                }
            }

            object obj = VariableExp.Evaluate();
            type = obj.GetType();

            // Case 2: "user.name"   -> property of map/object.
            if (obj is LMap)
            {
                if (IsAssignment)
                    return new Tuple<LMap, string>((LMap)obj, MemberName);
                else
                    return ((LMap)obj).ExecuteMethod(MemberName, null);
            }
            // Case 2a: string.Method
            if (obj is string)
            {
                return new Tuple<LString, string, string>(null, obj.ToString(), MemberName);
            }
            // Case 2b: date.Method
            if (obj is DateTime)
            {
                return new Tuple<LDate, string, object, string>(null, variableName, obj, MemberName);
            }

            // Case 3: "user.name" -> property on object user.
            MemberInfo[] members = type.GetMember(MemberName);
            MemberInfo result = null;
            if (obj is LArray && (members == null || members.Length == 0))
            {
                MemberName = LArray.MapMethod(MemberName);
                members = type.GetMember(MemberName);
            }
            result = members[0];

            if (result.MemberType == MemberTypes.Property)
            {
                if (IsAssignment)
                    return new Tuple<object, string, string, PropertyInfo>(obj, null, MemberName, type.GetProperty(MemberName));
                else
                    return type.GetProperty(MemberName).GetValue(obj, null);
            }

            // Case 4: "user.create" -> method on object user.             
            if (result.MemberType == MemberTypes.Method)
            {
                string name = (VariableExp is VariableExpression)
                            ? ((VariableExpression)VariableExp).Name
                            : null;
                return new Tuple<object, string, string, MethodInfo>(obj, name, MemberName, type.GetMethod(MemberName));
            }

            return result;
        }
    }    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace XUCore.Script
{

    class FunctionHelper
    {
        /// <summary>
        /// Whether or not the name/member combination supplied is a script level function or an external C# function
        /// </summary>
        /// <param name="ctx">Context of script</param>
        /// <param name="name">Object name "Log"</param>
        /// <param name="member">Member name "Info" as in "Log.Info"</param>
        /// <returns></returns>
        public static bool IsInternalOrExternalFunction(Context ctx, string name, string member)
        {
            string fullName = name;
            if (!string.IsNullOrEmpty(member))
                fullName += "." + member;

            // Case 1: getuser() script function
            if (ctx.Functions.Contains(fullName) || ctx.ExternalFunctions.Contains(fullName))
                return true;

            return false;
        }


        /// <summary>
        /// Call internal/external script.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="name"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static object Call(Context ctx, string name, FunctionCallExpression exp)
        {
            // Case 1: Custom C# function blog.create blog.*
            if (ctx.ExternalFunctions.Contains(name))
                return ctx.ExternalFunctions.Call(name, exp);

            // Case 2: Script functions "createUser('john');" 
            return ctx.Functions.Call(exp);
        }


        /// <summary>
        /// Dynamically invokes a method call.
        /// </summary>
        /// <param name="ctx">Context of the script</param>
        /// <param name="obj">Instance of the object for which the method call is being applied.</param>
        /// <param name="datatype">The datatype of the object.</param>
        /// <param name="methodInfo">The method to call.</param>
        /// <param name="paramListExpressions">List of expressions representing parameters for the method call</param>
        /// <param name="paramList">The list of values(evaluated from expressions) to call.</param>
        /// <param name="resolveParams">Whether or not to resolve the parameters from expressions to values.</param>
        /// <returns></returns>
        public static object MethodCall(Context ctx, object obj, Type datatype, MethodInfo methodInfo, List<Expression> paramListExpressions, List<object> paramList, bool resolveParams = true)
        {
            // 1. Convert language expressions to values.
            if (resolveParams) ResolveParameters(paramListExpressions, paramList);

            // 2. Convert internal language types to c# code method types.
            ConvertArgs(paramList, methodInfo);

            // 3. Now get args as an array for method calling.
            object [] args = paramList.ToArray();

            // 4. Handle  params object[];
            if (methodInfo.GetParameters().Length == 1)
            {
                if (methodInfo.GetParameters()[0].ParameterType == typeof(object[]))
                    args = new object[] { args };
            }
            object result = methodInfo.Invoke(obj, args);
            return result;
        }


        /// <summary>
        /// Call the method.
        /// </summary>
        /// <param name="ctx">The context of the script.</param>
        /// <param name="info">The name of the method</param>
        /// <param name="paramListExpressions">The expressions to resolve as parameters</param>
        /// <param name="paramList">The list of parameters.</param>
        /// <returns></returns>
        public static object Call(Context ctx, Tuple<object, string, string, MethodInfo> info, List<Expression> paramListExpressions, List<object> paramList)
        {
            //var type = info.Item1.GetType();
            var obj = info.Item1;
            var varname = info.Item2;
            var memberName = info.Item3;
            var methodInfo = info.Item4;
            return Call(ctx, obj, varname, memberName, methodInfo, paramListExpressions, paramList);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx">The context of the script</param>
        /// <param name="varname">The name of the variable</param>
        /// <param name="obj">The object to call the method on</param>
        /// <param name="memberName">The name of the member/method to call</param>
        /// <param name="methodInfo">The methodinfo(not needed for built in types )</param>
        /// <param name="paramListExpressions">The expressions to resolve as parameters</param>
        /// <param name="paramList">The list of parameters.</param>
        /// <returns></returns>
        public static object Call(Context ctx, object obj, string varname, string memberName, MethodInfo methodInfo, List<Expression> paramListExpressions, List<object> paramList)
        {
            // 1. Resolve the parameters.
            FunctionHelper.ResolveParameters(paramListExpressions, paramList);

            object result = null;
            Type type = obj.GetType();

            // 1. DateTime
            if (type == typeof(DateTime))
            {
                result = new LDate(ctx, varname, (DateTime)obj).ExecuteMethod(memberName, paramList.ToArray());
            }
            // 2. String
            else if (type == typeof(string))
            {
                result = new LString(ctx, varname, (string)obj).ExecuteMethod(memberName, paramList.ToArray());
            }
            // 3. Method info supplied
            else if (methodInfo != null)
            {
                result = MethodCall(ctx, obj, null, methodInfo, paramListExpressions, paramList, false);
            }
            else
            {
                methodInfo = type.GetMethod(memberName);
                if (methodInfo != null)
                    result = methodInfo.Invoke(obj, paramList.ToArray());
                else
                {
                    var prop = type.GetProperty(memberName);
                    if (prop != null)
                        result = prop.GetValue(obj, null);
                }
            }
            return result;
        }


        /// <summary>
        /// Resolve the parameters in the function call.
        /// </summary>
        public static void ResolveParameters(List<Expression> paramListExpressions, List<object> paramList)
        {
            if (paramListExpressions == null || paramListExpressions.Count == 0)
                return;

            paramList.Clear();
            foreach (var exp in paramListExpressions)
            {
                object val = exp.Evaluate();
                paramList.Add(val);
            }
        }


        /// <summary>
        /// Prints to the console.
        /// </summary>
        /// /// <param name="settings">Settings for interpreter</param>
        /// <param name="exp">The functiona call expression</param>
        /// <param name="printline">Whether to print with line or no line</param>
        public static string Print(LangSettings settings, FunctionCallExpression exp, bool printline)
        {
            if (!settings.EnablePrinting) return string.Empty;

            string message = BuildMessage(exp.ParamList);
            if (printline) Console.WriteLine(message);
            else Console.Write(message);
            return message;
        }


        /// <summary>
        /// Logs severity to console.
        /// </summary>
        /// <param name="settings">Settings for interpreter</param>
        /// <param name="exp">The functiona call expression</param>
        public static string Log(LangSettings settings, FunctionCallExpression exp)
        {
            if (!settings.EnableLogging) return string.Empty;

            string severity = exp.Name.Substring(exp.Name.IndexOf(".") + 1);
            string message = BuildMessage(exp.ParamList);
            Console.WriteLine(severity + " : " + message);
            return message;
        }


        /// <summary>
        /// Builds a single message from multiple arguments
        /// If there are 2 or more arguments, the 1st is a format, then rest are the args to the format.
        /// </summary>
        /// <param name="paramList">The list of parameters</param>
        /// <returns></returns>
        public static string BuildMessage(List<object> paramList)
        {
            string val = string.Empty;
            bool hasFormat = false;
            string format = string.Empty;
            if (paramList != null && paramList.Count > 0)
            {
                // Check for 2 arguments which reflects formatting the printing.
                hasFormat = paramList.Count > 1;
                if (hasFormat)
                {
                    format = paramList[0].ToString();
                    var args = paramList.GetRange(1, paramList.Count - 1);
                    val = string.Format(format, args.ToArray());
                }
                else
                    val = paramList[0].ToString();
            }
            return val;
        }


        /// <summary>
        /// Converts arguments from one type to another type that is required by the method call.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="method">The method for which the parameters need to be converted</param>
        public static void ConvertArgs(List<object> args, MethodInfo method)
        {
            var parameters = method.GetParameters();
            if (parameters == null || parameters.Length == 0) return;

            // For each param
            for (int ndx = 0; ndx < parameters.Length; ndx++)
            {
                var param = parameters[ndx];
                object sourceArg = args[ndx];

                // types match ? continue to next one.
                if (sourceArg.GetType() == param.ParameterType)
                    continue;

                // 1. Double to Int32
                if (sourceArg.GetType() == typeof(double) && param.ParameterType == typeof(int))
                    args[ndx] = Convert.ToInt32(sourceArg);

                // 2. LDate to datetime
                else if (sourceArg is LDate)
                    args[ndx] = ((LDate)sourceArg).Raw;

                // 3. LArray
                else if ((sourceArg is LArray || sourceArg is List<object>) && param.ParameterType.IsGenericType)
                {
                    if (sourceArg is LArray) sourceArg = ((LArray)sourceArg).Raw;
                    var gentype = param.ParameterType.GetGenericTypeDefinition();
                    if (gentype == typeof(List<>) || gentype == typeof(IList<>))
                    {
                        args[ndx] = ConvertToTypedList((List<object>)sourceArg, param.ParameterType);
                    }
                }
            }
        }


        /// <summary>
        /// Converts the source to the target list type by creating a new instance of the list and populating it.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetListType"></param>
        /// <returns></returns>
        static object ConvertToTypedList(IList<object> source, Type targetListType)
        {
            var t = targetListType; // targetListType.GetType();
            var dt = targetListType.GetGenericTypeDefinition();
            var targetType = dt.MakeGenericType(t.GetGenericArguments()[0]);
            var targetList = Activator.CreateInstance(targetType);
            System.Collections.IList l = targetList as System.Collections.IList;
            foreach (var item in source) l.Add(item);
            return targetList;
        }


        /// <summary>
        /// Converts the source to the target list type by creating a new instance of the list and populating it.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="targetListType"></param>
        /// <returns></returns>
        static object ConvertToTypedDictionary(IDictionary<string, object> source, Type targetListType)
        {
            var t = targetListType; // targetListType.GetType();
            var dt = targetListType.GetGenericTypeDefinition();
            var targetType = dt.MakeGenericType(t.GetGenericArguments()[0], t.GetGenericArguments()[1]);
            var targetDict = Activator.CreateInstance(targetType);
            System.Collections.IDictionary l = targetDict as System.Collections.IDictionary;
            foreach (var item in source) l.Add(item.Key, item.Value);
            return targetDict;
        }
    }
}

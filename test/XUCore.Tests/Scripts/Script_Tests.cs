using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Script;
using Xunit;

namespace XUCore.Tests.Scripts
{

    public class ScriptTestsBase
    {
        protected void Parse(List<Tuple<string, Type, object, string>> statements, bool execute = true, Action<Interpreter> initializer = null)
        {
            for (int ndx = 0; ndx < statements.Count; ndx++)
            {
                var stmt = statements[ndx];
                var i = new Interpreter();
                if (initializer != null)
                    initializer(i);

                if (execute)
                {
                    i.Execute(stmt.Item4);
                    Assert.Equal(i.Scope[stmt.Item1], stmt.Item3);
                }
                else
                {
                    i.Parse(stmt.Item4);
                }
            }
        }


        protected void ParseFuncCalls(List<Tuple<string, int, Type, object, string>> statements)
        {
            for (int ndx = 0; ndx < statements.Count; ndx++)
            {
                var stmt = statements[ndx];
                var i = new Interpreter();
                object result = null;

                string funcCallTxt = stmt.Item5;

                // Handle calls to "user.create".
                i.SetFunctionCallback(stmt.Item1, (exp) =>
                {
                    // 1. Check number of parameters match
                    Assert.Equal(exp.ParamList.Count, stmt.Item2);

                    // 2. Check name of func
                    Assert.Equal(exp.Name, stmt.Item1);

                    if (stmt.Item2 > 0)
                    {
                        // 3. return the type
                        result = exp.ParamList[Convert.ToInt32(exp.ParamList[0])];
                        return result;
                    }
                    result = 1;
                    return result;
                });

                i.Execute(funcCallTxt);

                // 4. Check return value
                Assert.Equal(result, stmt.Item4);
            }
        }
    }

    public class Script_Tests_Assignment : ScriptTestsBase
    {
        [Fact]
        public void Can_Do_Single_Assignment_Constant_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("name", typeof(object), LNull.Instance,  "var name;"),
                new Tuple<string,Type, object, string>("name", typeof(string), "kishore",       "var name = 'kishore';"),
                new Tuple<string,Type, object, string>("name", typeof(string), "kishore",       "var name = \"kishore\";"),
                new Tuple<string,Type, object, string>("age", typeof(double),   32,             "var age = 32;"),
                new Tuple<string,Type, object, string>("isActive", typeof(bool), true,          "var isActive = true;"),
                new Tuple<string,Type, object, string>("isActive", typeof(bool), false,         "var isActive = false;"),
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Multiple_Assignment_Constant_Expressions_In_Same_Line()
        {
            var statements = new List<Tuple<List<string>, List<Type>, List<object>, string>>()
            {
                new Tuple<List<string>, List<Type>, List<object>, string>(new List<string>(){ "name", "age", "isActive"}, new List<Type>(){ typeof(string), typeof(double), typeof(bool)}, new List<object>(){ "kis", 32,   true },  "var name = 'kis', age = 32, isActive = true;"),
                new Tuple<List<string>, List<Type>, List<object>, string>(new List<string>(){ "name", "age", "isActive"}, new List<Type>(){ typeof(string), typeof(double), typeof(bool)}, new List<object>(){ "kis", 32,   LNull.Instance},   "var name = 'kis', age = 32, isActive;"),
                new Tuple<List<string>, List<Type>, List<object>, string>(new List<string>(){ "name", "age", "isActive"}, new List<Type>(){ typeof(string), typeof(double), typeof(bool)}, new List<object>(){ "kis", LNull.Instance, LNull.Instance },  "var name = 'kis', age, isActive;"),
                new Tuple<List<string>, List<Type>, List<object>, string>(new List<string>(){ "name", "age", "isActive"}, new List<Type>(){ typeof(string), typeof(double), typeof(bool)}, new List<object>(){ LNull.Instance,  LNull.Instance, LNull.Instance },  "var name, age, isActive;"),
            };
            for (int ndx = 0; ndx < statements.Count; ndx++)
            {
                var stmt = statements[ndx];
                var i = new Interpreter();
                i.Execute(stmt.Item4);
                for (int ndxV = 0; ndxV < stmt.Item1.Count; ndxV++)
                {
                    string varName = stmt.Item1[ndxV];
                    Type type = stmt.Item2[ndxV];
                    object val = stmt.Item3[ndxV];
                    var actualValue = i.Scope.Get<object>(varName);

                    // Check values are correct.
                    Assert.Equal(val, actualValue);
                }
            }
        }


        [Fact]
        public void Can_Do_Single_Assignment_Constant_Math_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 8,  "var result = 4 * 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,  "var result = 6 / 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6,  "var result = 4 + 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,  "var result = 4 - 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var result = 5 % 2;")

            };
            Parse(statements);
        }


        [Fact]
        public void Can_Handle_Escape_Chars_InString()
        {
            var i = new Interpreter();
            i.Execute("var buffer = \"\r\n<h1 class=\\\"title\\\">\";");
            var s = i.Scope.Get<string>("buffer");
            //File.WriteAllText(@"c:\temp\jsnewlines.txt", s);
            //Assert.Equal(s, Environment.NewLine);
        }


        [Fact]
        public void Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 10,  "var result = 4 + 2 * 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 7,   "var result = 4 + 2 * 3 / 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 10,  "var result = 4 * 2 + 8 / 4;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6,   "var result = 4 * 8 / 8 + 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4,   "var result = 4 - 2 + 8 / 4;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,   "var result = 6 - 2 * 8 / 4;"),
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Single_Assignment_Constant_Math_Expressions_With_Precendence_With_Parenthesis()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 18,  "var result = (4 + 2) * 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 12,  "var result = (4 + 2) * (4 / 2);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 10,  "var result = 4 * (2 + 8) / 4;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6,   "var result = 4 * (8 / 8) + 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), -1,  "var result = 4 - (2 + 8) / 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 8,   "var result = (6 - 2) * 8 / 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool),   true,"var result = (6 - 2) > 4 || ( 1 > 2 || 4 > 3 );"),
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Single_Assignment_Constant_Math_Expressions_With_Mixed_Types()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "comlibext",   "var result = 'comlib' + 'ext';"),
                new Tuple<string,Type, object, string>("result", typeof(string), "comlib2",     "var result = 'comlib' + 2;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "3comlib",     "var result = 3 + 'comlib';"),
                new Tuple<string,Type, object, string>("result", typeof(string), "comlibtrue",  "var result = 'comlib' + true;"),
                new Tuple<string,Type, object, string>("result", typeof(string), "comlibfalse", "var result = 'comlib' + false;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,             "var result = 2 + false;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,             "var result = 2 + true;" ),
                new Tuple<string,Type, object, string>("result", typeof(double), "comlibnet4.5","var result = 'comlib' + 'net' + 4.5;" ),

            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Complex_Addition_On_Mixed_Types()
        {
            string text = @"var age = 30.5; "
                        + "var isActive = true; "
                        + "var name = 'john'; "
                        + "var date = new Date(); "
                        + "var ids = [10,11,12]; "
                        + "var addy = { City: 'Queens', State: 'NY' }; "
                        + "var result = name + age + isActive + date + ids[1] + addy.City;";
            var i = new Interpreter();
            i.Execute(text);
            var result = i.Scope.Get<string>("result");

            // john30.5true8/17/2011 3:14:25 PM11Queens
            Assert.StartsWith("john30.5true", result);
            Assert.EndsWith("11Queens", result);
            Assert.Contains(DateTime.Now.Date.ToShortDateString(), result);
        }


        [Fact]
        public void Can_Do_Single_Assignment_Constant_Logical_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 >  2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 >= 2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 4 <  2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 4 <= 2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 2 != 2 || 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 2 == 4 || 3 < 4;"),

                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 1 >  2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 1 >= 2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 4 <  2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 4 <= 2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 2 != 2 || 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 2 == 4 || 3 > 4;"),

                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 <  2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 <= 2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 4 >= 2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 1 <= 2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 2 == 2 && 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,  "var result = 2 != 4 && 3 < 4;"),

                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 1 <  2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 1 <= 2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 4 >= 2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 4 <= 2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 2 == 2 && 3 == 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false, "var result = 2 <  4 && 3 == 4;")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Multiple_Assignment_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result2", typeof(string), "kishore", "var result = 'kishore'; var result2 = result;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 8,         "var result = 4; var result2 = result * 2;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 3,         "var result = 6; var result2 = result / 2;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 6,         "var result = 4; var result2 = result + 2;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 2,         "var result = 4; var result2 = result - 2;"),
                new Tuple<string,Type, object, string>("result2", typeof(double), 1,         "var result = 5; var result2 = result % 2;"),
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Unary_Expressions()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), 3, "var result = 2; result++; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; result--; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 2; result += 2; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; result -= 2; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 6, "var result = 2; result *= 3; "),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 6; result /= 2; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   false, "var result = true; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   true,  "var result = false; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   false, "var result = 'abc'; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(bool),   false, "var result = 1; result = !result; "),
                new Tuple<string,Type, object, string>("result", typeof(string), "abcdef", "var result = 'abc'; result += 'def'; "),
            };
            Parse(statements);
        }
    }

    public class Script_Tests_Comparisons : ScriptTestsBase
    {
        [Fact]
        public void Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Numbers()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 >  2;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 >= 2;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 <  6;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 <= 6;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 != 2;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 4 == 4;"),
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Strings()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'a' == 'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'a' == 'b';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'a' != 'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'a' != 'b';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'a' <  'c';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'b' <  'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'a' >  'c';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' >  'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' <= 'b';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' <= 'c';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'b' <= 'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' >= 'b';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = 'b' >= 'a';"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = 'b' >= 'c';"),
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Single_Assignment_Constant_Compare_Expressions_On_Bools()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true == true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true == false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true != true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true != false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true <  true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true <  false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true >  true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true >  false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true <= true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,"var result = true <= false;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true >= true;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true, "var result = true >= false;")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Check_For_Nulls_Using_Complex_DataTypes()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; var users = ['a', null, 'b'];               if(users[1] == null)   result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; var users = ['a', null, 'b'];               if(users[0] != null)   result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; var user = { name: 'kishore', age : null }; if(user.age == null )  result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; var user = { name: 'kishore', age : 32 };   if(user.age != null )  result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; function add(a) { return null; }  if( add(1) == null) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; function add(a) { return 1; }     if( add(1) != null) result = 1;"),
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Check_For_Nulls_Using_Variables_Constants()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result ;    if(result == null) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(result != null) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result;     if(null == result) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(null != result) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(null == null)   result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 1; if(null != null)   result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if('abc' != null)  result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(true != null)   result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(false != null)  result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if(35 != null)     result = 1;")
            };
            Parse(statements);
        }
    }

    public class Script_Tests_IfConditions : ScriptTestsBase
    {
        [Fact]
        public void Can_Do_If_Statements_With_Constants()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 1; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 2; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 3; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 4; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 5, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 5; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6, "var result = 1; if( 2 < 3 && 4 > 3 ){ result = 6; }")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_If_Statements_With_Constants_Single_line()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 2;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 4;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 5, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 5;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 6, "var result = 1; if( 2 < 3 && 4 > 3 ) result = 6;"),
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_If_Else_Statements_With_Constants()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; if( 2 < 3 && 4 > 3 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; if( 2 < 3 && 4 > 3 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 2; if( 2 < 3 && 4 > 3 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; if( 2 < 3 && 4 > 5 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; if( 2 < 3 && 4 > 5 ) result = 1; else result = 0;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 0, "var result = 2; if( 2 < 3 && 4 > 5 ) result = 1; else result = 0;"),
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_If_ElseIf_Statements_With_Constants()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) result = 1; else if ( 3 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; if( 2 < 3 ) result = 1; else if ( 3 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 0; if( 3 < 3 ) result = 1; else if ( 3 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 0; if( 3 < 3 ) result = 1; else if ( 3 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; if( 3 < 3 ) result = 1; else if ( 4 < 4 ) result = 2; else result = 3;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; if( 3 < 3 ) result = 1; else if ( 4 < 4 ) result = 2; else result = 3;")
            };
            Parse(statements);
        }
    }

    public class Script_Tests_Loops : ScriptTestsBase
    {

        [Fact]
        public void Can_Do_While_Statements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; while( result < 4 ){ result++; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 4; while( result > 1 ){ result--; }")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_For_Loop_Statements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; for(var ndx = 0; ndx < 5; ndx++) { result = ndx; }")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_For_Each_Statements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 2,     "var result = 0; var ids = [0,1,2]; for(x in ids) { result = x; }"),
                new Tuple<string,Type, object, string>("result", typeof(string), "com", "var result = 0; var ids = {a:'com', b:'com', c:'com'}; for(x in ids) { result = x.Value; }")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Break_Statements()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 3; while( result < 4 ){ if( result > 2 ) { break; } result++; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 2; while( result > 1 ){ if( result < 3 ) { break; } result--; }"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 3; for(var ndx = 0; ndx < 5; ndx++) { if( result > 2 ) { break; } result = ndx; }")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Nested_Loops()
        {
            var i = new Interpreter();
            i.Execute("var result = 0; for(var a = 0; a < 10; a++) { for(var b = 0; b < 10; b++) { result++; } }");

            int result = i.Scope.Get<int>("result");
            Assert.Equal(100, result);
        }


        [Fact]
        public void Can_Do_Recursion()
        {
            var i = new Interpreter();
            i.Execute("function Additive(n) { if (n == 0 )  return 0; return n + Additive(n-1); } var result = Additive(5);");

            int result = i.Scope.Get<int>("result");
            Assert.Equal(15, result);
        }
    }

    public class Script_Tests_Functions : ScriptTestsBase
    {
        [Fact]
        public void Can_Make_Function_Script_Calls()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1,     "var result = 0; function test()     {  return 1;         } result = test();"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2,     "var result = 1; function test(a)    { return a + 1;      } result = test(1);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 4,     "var result = 2; function test(a)    { return a + result; } result = test(2);"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,    "var result = 1; function test(a)    { return true;       } result = test(1);"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,   "var result = 1; function test(a)    { return false;      } result = test(1);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 5,     "var result = 1; function test(a, b) { return a + b;      } result = test(2,3);"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3,     "var result = 1; function test(a, b) { return a - b;      } result = test(4,1);"),
                new Tuple<string,Type, object, string>("result", typeof(string), "com", "var result = 1; function test()     { return 'com';      } result = test();")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Make_Function_Calls_Inside_External_Parenthesis()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1, "var result = 0; function inc(a) { return a + 1; }  if( inc(1) == 2 ) result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 0; var b = 0; function inc(a) { return a + 1; }  while( inc(b) < 3 ){ b++; result = b;}"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; function inc(a) { return a + 1; }  result = inc(inc(1));"),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 0; function inc(a) { return a + 1; }  if( inc(inc(1)) == 3 ) result = 3;")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Try_Catch()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string), "test error", "var result = 'default'; try { throw 'test error'; } catch(err) { result = err.message; }")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Funcion_Calls_As_Statements()
        {
            // Tuple ( 0, 1, 2, 3, 4 )
            //         name, number of parameters, return type, return value, function call as string
            var statements = new List<Tuple<string, int, Type, object, string>>()
            {
                new Tuple<string, int, Type, object, string>("user.create", 0, typeof(double), 1,        "user.create();"),
                new Tuple<string, int, Type, object, string>("user.create", 5, typeof(double), "comlib", "user.create (1,  'comlib',  123, true,  30.5);"),
                new Tuple<string, int, Type, object, string>("user.create", 5, typeof(double), 123,      "user.create(2, \"comlib\", 123, false, 30.5);"),
                new Tuple<string, int, Type, object, string>("user.create", 5, typeof(bool),   true,     "user.create (3, \"comlib\", 123, true,  30.5);"),
                new Tuple<string, int, Type, object, string>("user.create", 5, typeof(double), 30.5,     "user.create  (4, \"comlib\", 123, false, 30.5);")
            };
            ParseFuncCalls(statements);
        }


        [Fact]
        public void Can_Do_Funcion_Calls_As_Expressions()
        {
            // Tuple ( 0, 1, 2, 3, 4 )
            //         name, number of parameters, return type, return value, function call as string
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(double), 1,        "var result = user.create();"),
                new Tuple<string, Type, object, string>("result", typeof(double), "comlib", "var result = user.create (1,  'comlib',  123, true,  30.5);"),
                new Tuple<string, Type, object, string>("result", typeof(double), 123,      "var result = user.create(2, \"comlib\", 123, false, 30.5);"),
                new Tuple<string, Type, object, string>("result", typeof(bool),   true,     "var result = user.create (3, \"comlib\", 123, true,  30.5);"),
                new Tuple<string, Type, object, string>("result", typeof(double), 30.5,     "var result = user.create  (4, \"comlib\", 123, false, 30.5);")
            };
            Parse(statements, true, (interpreter) =>
            {
                interpreter.SetFunctionCallback("user.create", (exp) =>
                {
                    if (exp.ParamList.Count == 0) return 1;

                    int indexOfparam = Convert.ToInt32(exp.ParamList[0]);
                    return exp.ParamList[indexOfparam];
                });
            });
        }


        [Fact]
        public void Can_Do_Function_Calls_With_Mixed_Types()
        {
            string text = "function add(name, age, date, ids, isActive, addy) { var result2 = name + age + isActive + date + ids[1] + addy.City; return result2; }"
                        + "var result = add('john', 30.5, new Date(), [10,11,12], true, { City: 'Queens', State: 'NY' });";
            var i = new Interpreter();
            i.Execute(text);
            var result = i.Scope.Get<string>("result");

            // john30.5true8/17/2011 3:14:25 PM11Queens
            Assert.StartsWith("john30.5true", result);
            Assert.EndsWith("11Queens", result);
            Assert.Contains(DateTime.Now.Date.ToShortDateString(), result);
        }


        [Fact]
        public void Can_Make_Function_Script_Calls_Using_Run_Statement()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 1; function 'add 1'    { result = result + 1; } run 'add 1'(); "),
                new Tuple<string,Type, object, string>("result", typeof(double), 3, "var result = 1; function 'add 2'    { result = result + 2; } run function 'add 2'(); "),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 1; function 'add 3'    { return result + 3; } result = run 'add 3'(); "),
                new Tuple<string,Type, object, string>("result", typeof(double), 5, "var result = 1; function 'add 4'    { return result + 4; } result = run function 'add 4'(); "),

                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 0; function 'add 3'(a) { return a + 3;   } result = run 'add 3'(1); "),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 0; function 'add 3'(a) { return a + 3;   } result = run function 'add 3'(1); "),
                new Tuple<string,Type, object, string>("result", typeof(double), 2, "var result = 0; function 'add 1'(a) { result = a + 1; } run 'add 1'(1); "),
                new Tuple<string,Type, object, string>("result", typeof(double), 4, "var result = 0; function 'add 3'(a) { result = a + 3; } run function 'add 3'(1); ")

            };
            Parse(statements);
        }
    }
    public class Script_Tests_Types : ScriptTestsBase
    {
        [Fact]
        public void Can_Do_Single_Assignment_New_Expressions()
        {
            var i = new Interpreter();
            i.Context.Types.Register(typeof(Person), null);
            i.Execute("var result = new Person();");
            Assert.Equal(typeof(Person), i.Scope.Get<object>("result").GetType());

            i.Execute("var result = new Date();");
            Assert.Equal(typeof(DateTime), i.Scope.Get<object>("result").GetType());
        }


        [Fact]
        public void Can_Do_Array_Declarations()
        {
            var statements = new List<Tuple<string, Type, Type, int, string>>()
            {
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(string),   0,  "var result = []; "),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(string),   1,  "var result = ['a']; "  ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(double),   2,  "var result = [1,    2]; "   ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(bool),     3,  "var result = [true, false, true];" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray),  typeof(double),  4,  "var result = [1.1,  2.2,   3.33, 4.44];" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(DateTime), 5,  "var result = [new Date(),  new Date(), new Date(), new Date(), new Date()];" )
            };
            foreach (var stmt in statements)
            {
                var i = new Interpreter();
                i.Execute(stmt.Item5);
                var array = i.Scope.Get<LArray>("result");

                Assert.Equal(array.Length, stmt.Item4);
            }
        }


        [Fact]
        public void Can_Do_Array_Method_Calls()
        {
            var arrytext = "var arr = ['a', 'b', 'c', 'd'];";
            var statements = new List<Tuple<string, Type, object, int, string>>()
            {
                new Tuple<string, Type, object, int, string>("result", typeof(int),    4,           4,  arrytext + " var result = arr.length;"),
                new Tuple<string, Type, object, int, string>("result", typeof(string), "e",         5,  arrytext + " arr.push('e'); var result = arr[4];"),
                new Tuple<string, Type, object, int, string>("result", typeof(string), "d",         3,  arrytext + " var result = arr.pop();"),
                new Tuple<string, Type, object, int, string>("result", typeof(string), "a;b;c;d",   4,  arrytext + " var result = arr.join(';');"),
                new Tuple<string, Type, object, int, string>("result", typeof(string), "d;c;b;a",   4,  arrytext + " var result = arr.reverse().join(';');")
            };
            foreach (var stmt in statements)
            {
                var i = new Interpreter();
                i.Execute(stmt.Item5);
                var result = i.Scope.Get<object>("result");

                // Check type
                Assert.Equal(result.GetType(), stmt.Item3.GetType());

                // Check value
                Assert.Equal(result, stmt.Item3);

                // Check length
                LArray arr = i.Scope.Get<LArray>("arr");
                Assert.Equal(stmt.Item4, arr.Length);
            }
        }


        private void Can_Do_Map_Declarations()
        {
            var statements = new List<Tuple<string, Type, Type, int, string>>()
            {
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(string),   0,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(string),   1,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(double),   2,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(bool),     3,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray),  typeof(double),  4,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" ),
                new Tuple<string, Type, Type, int, string>("result", typeof(LArray), typeof(DateTime), 5,  "var result = { name: 'kishore', id: 123, isactive: true, dt: new Date(), salary: 80.5 };" )
            };
            foreach (var stmt in statements)
            {
                var i = new Interpreter();
                i.Execute(stmt.Item5);
                var array = i.Scope.Get<LArray>("result");

                Assert.Equal(array.Length, stmt.Item4);
            }
        }


        [Fact]
        public void Can_Do_String_Method_Calls()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),   "common@lib",  "var name = 'COmmOn@Lib'; var result = name.toLowerCase();"),
                new Tuple<string,Type, object, string>("result", typeof(string),   "COMMON@LIB",  "var name = 'COmmOn@Lib'; var result = name.toUpperCase();"),
                new Tuple<string,Type, object, string>("result", typeof(int),      6,             "var name = 'COmmOn@Lib'; var result = name.indexOf('@');"),
                new Tuple<string,Type, object, string>("result", typeof(int),      4,             "var name = 'COmmOn@Lib'; var result = name.lastIndexOf('O');"),
                new Tuple<string,Type, object, string>("result", typeof(string),   "mmOn",        "var name = 'COmmOn@Lib'; var result = name.substr(2, 4);"),
                new Tuple<string,Type, object, string>("result", typeof(string),   "mmO",         "var name = 'COmmOn@Lib'; var result = name.substring(2, 4);"),
                new Tuple<string,Type, object, string>("result", typeof(string),   "COnnOn@Lib",  "var name = 'COmmOn@Lib'; var result = name.replace('mm', 'nn');")
            };
            Parse(statements);
        }


        [Fact]
        public void Can_Do_Date_Method_Calls()
        {
            var date = DateTime.Now;
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(int), date.Day,                             "var result = date.getDate();"),
                new Tuple<string,Type, object, string>("result", typeof(int), (int)date.DayOfWeek,                  "var result = date.getDay();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.Month,                           "var result = date.getMonth();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.Year,                            "var result = date.getFullYear();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.Hour,                            "var result = date.getHours();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.Minute,                          "var result = date.getMinutes();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.Second,                          "var result = date.getSeconds();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.ToUniversalTime().Day,           "var result = date.getUTCDate();"),
                new Tuple<string,Type, object, string>("result", typeof(int), (int)date.ToUniversalTime().DayOfWeek,"var result = date.getUTCDay();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.ToUniversalTime().Month,         "var result = date.getUTCMonth();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.ToUniversalTime().Year,          "var result = date.getUTCFullYear();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.ToUniversalTime().Hour,          "var result = date.getUTCHours();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.ToUniversalTime().Minute,        "var result = date.getUTCMinutes();"),
                new Tuple<string,Type, object, string>("result", typeof(int), date.ToUniversalTime().Second,        "var result = date.getUTCSeconds();"),

                new Tuple<string,Type, object, string>("result", typeof(int), 10,       "date.setDate(10);       var result = date.getDate();"),
                new Tuple<string,Type, object, string>("result", typeof(int), 3,        "date.setMonth(3);       var result = date.getMonth();"),
                new Tuple<string,Type, object, string>("result", typeof(int), 2010,     "date.setFullYear(2010); var result = date.getFullYear();"),
                new Tuple<string,Type, object, string>("result", typeof(int), 14,       "date.setHours(14);      var result = date.getHours();"),
                new Tuple<string,Type, object, string>("result", typeof(int), 30,       "date.setMinutes(30);    var result = date.getMinutes();"),
                new Tuple<string,Type, object, string>("result", typeof(int), 45,       "date.setSeconds(45);    var result = date.getSeconds();")
            };
            Parse(testcases, true, (i) => i.Scope.SetValue("date", date));
        }
    }

    public class Script_Tests_MemberAccess : ScriptTestsBase
    {
        private void RunTestCases(List<Tuple<string, Type, object, string>> testcases)
        {
            for (int ndx = 0; ndx < testcases.Count; ndx++)
            {
                var test = testcases[ndx];
                var i = new Interpreter();
                i.Context.Types.Register(typeof(Person), null);
                i.Execute(test.Item4);

                // 1. Check type of result is correct
                Assert.Equal(i.Scope.Get<object>(test.Item1).GetType(), test.Item2);

                // 2. Check that value is correct
                object actualValue = i.Scope.Get<object>(test.Item1);
                if (test.Item3 is DateTime)
                {
                    var actualDate = (DateTime)actualValue;
                    actualDate = actualDate.Date;
                    Assert.Equal(test.Item3, actualDate);
                }
                else
                    Assert.Equal(test.Item3, actualValue);
            }
        }



        [Fact]
        public void Can_Get_Class_Member_Property()
        {
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "john",              "var p = new Person(); var result = p.FirstName;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "johndoe@email.com", "var p = new Person(); var result = p.Email;" ),
                new Tuple<string, Type, object, string>("result", typeof(bool),     true,                "var p = new Person(); var result = p.IsMale;" ),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.5,                "var p = new Person(); var result = p.Salary;" ),
                new Tuple<string, Type, object, string>("result", typeof(DateTime), DateTime.Today,      "var p = new Person(); var result = p.BirthDate;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "Queens",            "var p = new Person(); var result = p.Address.City;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NY",                "var p = new Person(); var result = p.Address.State;" )
            };
            RunTestCases(testcases);
        }


        [Fact]
        public void Can_Set_Class_Member_Property()
        {
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "jane",              "var p = new Person();  p.FirstName = 'jane';     var result = p.FirstName;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "janedoe@email.com", "var p = new Person();  p.Email     = 'janedoe@email.com'; var result = p.Email;" ),
                new Tuple<string, Type, object, string>("result", typeof(bool),     false,               "var p = new Person();  p.IsMale    =  false;       var result = p.IsMale;" ),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.8,                "var p = new Person();  p.Salary    = 10.8;         var result = p.Salary;" ),
                new Tuple<string, Type, object, string>("result", typeof(DateTime), DateTime.Today,      "var p = new Person();  p.BirthDate = new Date();   var result = p.BirthDate;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "Bronx",             "var p = new Person();  p.Address.City = 'Bronx';   var result = p.Address.City;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NJ",                "var p = new Person();  p.Address.State = 'NJ';     var result = p.Address.State;" )
            };
            RunTestCases(testcases);
        }


        [Fact]
        public void Can_Get_Map_Member_Property()
        {
            string maptxt = @"{ FirstName: 'john', LastName: 'doe', Email: 'johndoe@email.com', IsMale: true, Salary: 10.5, BirthDate: new Date(), Address: { City: 'Queens', State: 'NY' } }";
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "john",              "var p = " + maptxt + "; var result = p.FirstName;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "johndoe@email.com", "var p = " + maptxt + "; var result = p.Email;" ),
                new Tuple<string, Type, object, string>("result", typeof(bool),     true,                "var p = " + maptxt + "; var result = p.IsMale;" ),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.5,                "var p = " + maptxt + "; var result = p.Salary;" ),
                new Tuple<string, Type, object, string>("result", typeof(DateTime), DateTime.Today,      "var p = " + maptxt + "; var result = p.BirthDate;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "Queens",            "var p = " + maptxt + "; var result = p.Address.City;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NY",                "var p = " + maptxt + "; var result = p.Address.State;" )
            };
            RunTestCases(testcases);
        }


        [Fact]
        public void Can_Set_Map_Member_Property()
        {
            string maptxt = @"{ FirstName: 'john', LastName: 'doe', Email: 'johndoe@email.com', IsMale: true, Salary: 10.5, BirthDate: new Date(), Address: { City: 'Queens', State: 'NY' } }";
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "jane",              "var p = " + maptxt + ";  p.FirstName = 'jane';     var result = p.FirstName;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "janedoe@email.com", "var p = " + maptxt + ";  p.Email     = 'janedoe@email.com'; var result = p.Email;" ),
                new Tuple<string, Type, object, string>("result", typeof(bool),     false,               "var p = " + maptxt + ";  p.IsMale    =  false;       var result = p.IsMale;" ),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.8,                "var p = " + maptxt + ";  p.Salary    = 10.8;         var result = p.Salary;" ),
                new Tuple<string, Type, object, string>("result", typeof(DateTime), DateTime.Today,      "var p = " + maptxt + ";  p.BirthDate = new Date();   var result = p.BirthDate;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "Bronx",             "var p = " + maptxt + ";  p.Address.City = 'Bronx';   var result = p.Address.City;" ),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NJ",                "var p = " + maptxt + ";  p.Address.State = 'NJ';     var result = p.Address.State;" )
            };
            RunTestCases(testcases);
        }


        [Fact]
        public void Can_Get_Array_ByIndex()
        {
            string maptxt = @"[ 'john', 'johndoe@email.com', true, 10.5, new Date(), { City: 'Queens', State: 'NY' }, [0, 1, 2] ]";
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "john",              "var p = " + maptxt + "; var result = p[0];"),
                new Tuple<string, Type, object, string>("result", typeof(string),   "johndoe@email.com", "var p = " + maptxt + "; var result = p[1];"),
                new Tuple<string, Type, object, string>("result", typeof(bool),     true,                "var p = " + maptxt + "; var result = p[2];"),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.5,                "var p = " + maptxt + "; var result = p[3];"),
                new Tuple<string, Type, object, string>("result", typeof(string),   "Queens",            "var p = " + maptxt + "; var result = p[5].City;"),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NY",                "var p = " + maptxt + "; var result = p[5].State;")
            };
            RunTestCases(testcases);
        }


        [Fact]
        public void Can_Get_Array_Item_By_Nested_Indexes()
        {
            string maptxt = @"var users = ['john1', 'john2', 'john3']; var indexes = [0, 1, 2]; ";
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "john2",  maptxt + " var result = users[indexes[1]];"),
                new Tuple<string, Type, object, string>("result", typeof(string),   "john3",  maptxt + " var result = users[indexes[1+1]];"),
                new Tuple<string, Type, object, string>("result", typeof(string),   "john3",  maptxt + " var result = users[indexes[1]+1];"),
            };
            RunTestCases(testcases);
        }


        [Fact]
        public void Can_Set_Array_ByIndex()
        {
            string maptxt = @"[ 'john', 'johndoe@email.com', true, 10.5, new Date(), { City: 'Queens', State: 'NY' }, [0, 1, 2] ]";
            var testcases = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string, Type, object, string>("result", typeof(string),   "jane",              "var p = " + maptxt + "; p[0] = 'jane';  var result = p[0];"),
                new Tuple<string, Type, object, string>("result", typeof(string),   "janedoe@email.com", "var p = " + maptxt + "; p[1] = 'janedoe@email.com'; var result = p[1];"),
                new Tuple<string, Type, object, string>("result", typeof(bool),     false,               "var p = " + maptxt + "; p[2] = false;   var result = p[2];"),
                new Tuple<string, Type, object, string>("result", typeof(double),   10.8,                "var p = " + maptxt + "; p[3] = 10.8;    var result = p[3];"),
                new Tuple<string, Type, object, string>("result", typeof(string),   "Bronx",             "var p = " + maptxt + "; p[5].City = 'Bronx';  var result = p[5].City;"),
                new Tuple<string, Type, object, string>("result", typeof(string),   "NJ",                "var p = " + maptxt + "; p[5].State = 'NJ';    var result = p[5].State;")
            };
            RunTestCases(testcases);
        }
    }

    public class Script_Tests_Limits : ScriptTestsBase
    {
        private void Run(List<string> scripts, Action<Interpreter, LangSettings> initializer = null, Action<Interpreter> assertCallback = null, bool isValid = false)
        {
            foreach (var script in scripts)
            {
                var i = new Interpreter();
                initializer?.Invoke(i, i.Context.Settings);

                i.Execute(script);

                if (isValid)
                    Assert.True(i.Result.Success);
                else
                {
                    Assert.False(i.Result.Success);
                    Assert.StartsWith("Limit Error", i.Result.Message);
                }
                assertCallback?.Invoke(i);
            }
        }


        [Fact]
        public void Can_Set_Loop_Limit()
        {
            Run(new List<string>() { "var result = 1; for(var ndx = 0; ndx < 20; ndx++) { result = ndx; }" },
                (i, settings) => settings.MaxLoopLimit = 10,
                i => Assert.Equal(10, i.Scope.Get<int>("result")));
        }


        /// <summary>
        /// Note: This actually works, because 
        /// 1. the loop limt of (10) fails after "result = ndx".
        /// 2. the exception is caught in the catch
        /// 3. the loop is executed again.
        /// </summary>
        [Fact]
        public void Can_Set_Loop_Limit_With_Try_Catch()
        {
            Run(new List<string>() { "var result = 0; try{ for(var ndx = result; ndx < 20; ndx++) { result = ndx; } } catch(err){ } " },
                (i, settings) => settings.MaxLoopLimit = 10,
                i => Assert.Equal(10, i.Scope.Get<int>("result")));
        }


        /// <summary>
        /// Note: This actually works, because 
        /// 1. the loop limt of (10) fails after "result = ndx".
        /// 2. the exception is caught in the catch
        /// 3. the loop is executed again.
        /// </summary>
        [Fact]
        public void Can_Set_Loop_Limit_With_Try_Catch_Then_Loop_Again()
        {
            var i = new Interpreter();
            i.Context.Settings.MaxLoopLimit = 10;
            i.Execute("var result = 0; try{ for(var ndx = result; ndx < 20; ndx++) { result = ndx; } } catch(err){ } "
                    + "for(var ndx = result; ndx < 20; ndx++) { result = ndx; } ");

            //Assert.Equal(true, i.Result.Success);
            Assert.False(i.Result.Success);
            Assert.StartsWith("Limit Error", i.Result.Message);
            Assert.Equal(10, i.Scope.Get<int>("result"));
        }


        [Fact]
        public void Can_Set_Recursion_Limit()
        {
            Run(new List<string>() { "function Additive(n) { if (n == 0 )  return 0; return n + Additive(n-1); } var result = Additive(6);" },
                (i, settings) => settings.MaxCallStack = 5,
                i => Assert.StartsWith("Limit Error", i.Result.Message));
        }


        [Fact]
        public void Can_Set_Call_Stack_Cyclic_Limit()
        {
            Run(new List<string>() { "function Add1(n) { var l = n + 1; return Add2(l); } function Add2(n){ var l = n + 2; Add1(l); } var result = Add1(1); " },
                (i, settings) => settings.MaxCallStack = 4,
                i => Assert.StartsWith("Limit Error", i.Result.Message));

            Run(new List<string>() { "function Add1(n) { var l = n + 1; return Add2(l); } function Add2(n){ if(n > 5 ) return n; var l = n + 2; Add1(l); } var result = Add1(1); " },
                null, (i) => Assert.Equal(8, i.Context.Scope.Get<int>("result")), true);
        }


        [Fact]
        public void Can_Set_Recursion_LimitLess()
        {
            Run(new List<string>() { "function Additive(n) { if (n == 0 )  return 0; return n + Additive(n-1); } var result = Additive(15);" },
                null,
                i => Assert.Equal(120, i.Scope.Get<int>("result")),
                true);
        }


        [Fact]
        public void Can_Set_Function_Parameters_Limit()
        {
            Run(new List<string>() { "function add(args){ return args[0]; } add(1,2,3,4,5,6,7,8,9);" },
                (i, settings) => settings.MaxFuncParams = 8);
        }


        [Fact]
        public void Can_Set_Statement_Limits()
        {
            Run(new List<string>() { "var a = 1; var b = 2; var c = 3; var d = 4; var e = 5;" },
                (i, settings) => settings.MaxStatements = 4);
        }


        [Fact]
        public void Can_Set_Statement_Nested_Limit()
        {
            var text = "var isActive = true; if(isActive){ while(isActive){ if(isActive) { try{ if(isActive) { isActive = false; } }catch(ex){ } } } } }";
            Run(new List<string>() { text }, (i, settings) => settings.MaxStatementsNested = 4);
        }


        [Fact]
        public void Can_Set_Member_Expression_Limits()
        {
            Run(new List<string>()
            {
                "var args = [0, 1, 2]; var result = args[0][1][2][3][4][5][6];",
                "var args = [0, 1, 2]; var result = args()()()()()()()()();",
                "var args = [0, 1, 2]; var result = args.a.b.c.d.e.f.g;",
                "var args = [0, 1, 2]; var result = args[0]().a[1]().b[2]();"
            },
            (i, settings) => settings.MaxConsequetiveMemberAccess = 5);
        }


        [Fact]
        public void Can_Set_Max_Expression_Limits()
        {
            Run(new List<string>()
            {
                "var args = 1 + 2 * 3 - 4 / 5 + 6;",
                "var arrs = [ [ [ [ [ [ ['5'],'4'],'3'],'2'],'1'],'0'], '-1'];",
                "var maps = { a: { b: { c: { d: { e: { f: 6 } } } } } };",
                //"var map  = { a: '1', b: '2', c: '3', d: '4', e: '5', f: '6' };"
            },
            (i, settings) => i.Context.Settings.MaxConsequetiveExpressions = 5,
            i => Assert.Contains("consequetive expressions (6)", i.Result.Message));
        }


        [Fact]
        public void Can_Set_Max_Nested_Function_Calls_As_Arguments()
        {
            Run(new List<string>()
            {
                "function add(n){ return n + 1; } var result = add(add(add(add(add(add(1))))));"
            },
            (i, settings) => { i.Context.Settings.MaxFuncCallNested = 5; });
        }


        [Fact]
        public void Can_Set_String_Limits()
        {
            Run(new List<string>()
            {
                "var name = 'kr'; for(var ndx = 0; ndx < 10; ndx++) name += name;",
                "var name = '0123456789'; for(var ndx = 0; ndx < 100; ndx++) name += '0123456789';",
                "var name = '0123456789'; for(var ndx = 0; ndx < 98; ndx++) name += '0123456789'; var result = name + '012345678901234567890123456789';",
            },
            (i, settings) => settings.MaxStringLength = 1000,
            i => Assert.StartsWith("Limit Error", i.Result.Message));
        }


        [Fact]
        public void Can_Set_Scope_Variables_Limit()
        {
            Run(new List<string>() { "var a = 1; var b = 2; var c = 3; var d = 4; var e = 5;" },
                (i, settings) => settings.MaxScopeVariables = 4);
        }


        [Fact]
        public void Can_Set_Exception_Limit()
        {
            Run(new List<string>() { "var a = 0; try { throw 'error'; } catch(err) { a = 1; } try { throw 'error2'; } catch(er2) { a = 2; }" },
                null, (i) => Assert.Equal(2, i.Context.Scope.Get<int>("a")), true);

            Run(new List<string>() { "var a = 0; try { throw 'error'; } catch(err) { a = 1; } try { throw 'error2'; } catch(er2) { a = 2; }" },
                (i, settings) => settings.MaxExceptions = 1, (i) => Assert.Equal(1, i.Context.Scope.Get<int>("a")));
        }


        [Fact]
        public void Can_Set_Scope_Variables_String_Length_Total_Limit()
        {
            Run(new List<string>() { "var a = '1234567890'; var b = '12345'; var c = 3; var d = '123456789012345'; var e = 5;" },
                (i, settings) => settings.MaxScopeStringVariablesLength = 15);
        }
    }

    public class Script_Tests_CustomObject : ScriptTestsBase
    {
        [Fact]
        public void Can_Create_Custom_Object_Via_Different_Constructors()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "john",                          "var p = new Person(); var result = p.FirstName;"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "kish",                          "var p = new Person('ki', 'sh'); var result = p.FirstName + p.LastName; "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "nonew@email.com",               "var p = new Person('john', 'doe', 'nonew@email.com', true, 10.56); var result = p.Email;  "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "janedallparams@email.comfalse", "var p = new Person('jane', 'd', 'allparams@email.com', false, 10.56, new Date()); var result = p.FirstName + p.LastName + p.Email + p.IsMale;")
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Fact]
        public void Can_Call_Custom_Object_Methods()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "ki sh",                 "var p = new Person('ki', 'sh', 'comlib@email.com', true, 12.34); var result = p.FullName();"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "programmer ki sh sr.",  "var p = new Person('ki', 'sh', 'comlib@email.com', true, 12.34); var result = p.FullNameWithPrefix('programmer', true);"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "k dog",                 "var result = Person.ToFullName('k', 'dog');")
            };
            Parse(statements, true, (i) => i.Context.Types.Register(typeof(Person), null));
        }


        [Fact]
        public void Can_Access_Custom_Object_Properties()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),    "john",                "var result = p.FirstName;"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "doe",                 "var result = p.LastName; "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "johndoe@email.com",   "var result = p.Email;    "),
                new Tuple<string,Type, object, string>("result", typeof(bool),      true,                  "var result = p.IsMale;   "),
                new Tuple<string,Type, object, string>("result", typeof(double),    10.56,                 "var result = p.Salary;   "),
                new Tuple<string,Type, object, string>("result", typeof(string),    "Queens",              "var result = p.Address.City;"),
                new Tuple<string,Type, object, string>("result", typeof(string),    "NY",                  "var result = p.Address.State;")
            };
            Parse(statements, true, (i) =>
            {
                i.Context.Types.Register(typeof(Person), null);
                i.Context.Scope.SetValue("p", new Person("john", "doe", "johndoe@email.com", true, 10.56, DateTime.Now.Date));
            });
        }
    }

    public class Script_Tests_ErrorHandling : ScriptTestsBase
    {
        [Fact]
        public void Can_Handle_Division_by_Zero()
        {
            var i = new Interpreter();
            i.Execute("var result = 5/0;");
            Assert.True(i.Result.Success);
        }


        [Fact]
        public void Can_Prevent_Instantiation_Of_Non_Registered_Types()
        {
            var result = true;
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(string),   "common@lib",  "var file = new File('c:\tmp\test.txt'); file.Delete();")
            };
            try { Parse(statements); }
            catch { result = false; }
            Assert.False(result);
        }


        [Fact]
        public void Can_Handle_Script_Syntax_Errors()
        {
            var statements = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("Syntax Error",  "var;"),
                new Tuple<string, string>("Syntax Error",  "while{};"),
                new Tuple<string, string>("Syntax Error",  "for( in users){}"),
                new Tuple<string, string>("Syntax Error",  "var users = [ ],];"),
                new Tuple<string, string>("Syntax Error",  "function Name a(b,c) { return null; }"),
                new Tuple<string, string>("Syntax Error",  "if else { print('ok'); }"),
                new Tuple<string, string>("Syntax Error : Unexpected end of script",  "var result = 0; result."),
                new Tuple<string, string>("Syntax Error",  "try print(1); catch(er){}")
            };
            foreach (var stmt in statements)
            {
                var i = new Interpreter();
                i.Execute(stmt.Item2);
                Assert.False(i.Result.Success);
                Assert.StartsWith(stmt.Item1, i.Result.Message);
            }
        }
    }

    public class Script_Tests_Subs : ScriptTestsBase
    {
        [Fact]
        public void Can_Do_Replace()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(bool), true,   "var result = 1 < 2 and 3 < 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,  "var result = 1 < 2 and 3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,   "var result = 1 < 2 or  3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,  "var result = 1 > 2 or  3 > 4;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), true,   "var result = yes;"),
                new Tuple<string,Type, object, string>("result", typeof(bool), false,  "var result = no;")

            };
            Parse(statements, true, i =>
            {
                i.LexReplace("and", "&&");
                i.LexReplace("or", "||");
                i.LexReplace("yes", "true");
                i.LexReplace("no", "false");
            });
        }


        [Fact]
        public void Can_Do_Remove()
        {
            var statements = new List<Tuple<string, Type, object, string>>()
            {
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "the var result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var the result = 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var result = the 1;"),
                new Tuple<string,Type, object, string>("result", typeof(double), 1,  "var result = 1 the;")
            };
            Parse(statements, true, i =>
            {
                i.LexRemove("the");
            });
        }
    }

    public class Person
    {
        public Person()
        {
            Init("john", "doe", "johndoe@email.com", true, 10.5, DateTime.Now);
        }


        public Person(string first, string last)
        {
            Init(first, last, first + last + "@email.com", true, 10.5, DateTime.Now);
        }


        public Person(string first, string last, string email, bool isMale, double salary)
        {
            Init(first, last, email, isMale, salary, DateTime.Now.Date);
        }


        public Person(string first, string last, string email, bool isMale, double salary, DateTime birthday)
        {
            Init(first, last, email, isMale, salary, birthday);
        }


        public void Init(string first, string last, string email, bool isMale, double salary, DateTime birthday)
        {
            FirstName = first;
            LastName = last;
            Email = email;
            IsMale = isMale;
            Salary = salary;
            BirthDate = birthday;
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public bool IsMale { get; set; }
        public double Salary { get; set; }

    }
}

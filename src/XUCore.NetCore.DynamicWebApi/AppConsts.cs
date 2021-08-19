using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XUCore.NetCore.DynamicWebApi
{
    public static class AppConsts
    {
        public static string DefaultHttpVerb { get; set; }

        public static string DefaultAreaName { get; set; }

        public static string DefaultApiPreFix { get; set; }

        public static List<string> ControllerPostfixes { get; set; }
        public static List<string> ActionPostfixes { get; set; }

        public static List<Type> FormBodyBindingIgnoredTypes { get; set; }

        public static Dictionary<string, string> HttpVerbs { get; set; }
        public static bool IsRemoveVerbs { get; set; }

        public static Func<string, string> GetRestFulActionName { get; set; }

        public static bool SplitControllerCamelCase { get; set; }

        public static string SplitControllerCamelCaseSeparator { get; set; }

        public static bool SplitActionCamelCase { get; set; }

        public static string SplitActionCamelCaseSeparator { get; set; }

        public static string VersionSeparator { get; set; }
        public static bool IsAutoSortAction { get; set; }

        public static Dictionary<Assembly, AssemblyDynamicWebApiOptions> AssemblyDynamicWebApiOptions { get; set; }

        static AppConsts()
        {
            HttpVerbs = new Dictionary<string, string>()
            {
                ["add"] = "POST",
                ["create"] = "POST",
                ["post"] = "POST",
                ["insert"] = "POST",
                ["submit"] = "POST",

                ["get"] = "GET",
                ["find"] = "GET",
                ["fetch"] = "GET",
                ["query"] = "GET",

                ["update"] = "PUT",
                ["put"] = "PUT",

                ["delete"] = "DELETE",
                ["remove"] = "DELETE",
                ["clear"] = "DELETE",
            };
        }
    }
}

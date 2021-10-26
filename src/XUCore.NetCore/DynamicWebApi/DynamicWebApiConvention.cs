using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using XUCore.NetCore.DynamicWebApi.Helper;

namespace XUCore.NetCore.DynamicWebApi
{
    public class DynamicWebApiConvention : IApplicationModelConvention
    {

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var type = controller.ControllerType.AsType();
                var dynamicWebApiAttr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicWebApiAttribute>(type.GetTypeInfo());
                if (typeof(IDynamicWebApi).GetTypeInfo().IsAssignableFrom(type))
                {
                    ConfigureArea(controller, dynamicWebApiAttr);
                    ConfigureController(controller, dynamicWebApiAttr);
                    ConfigureDynamicWebApi(controller, dynamicWebApiAttr);
                }
                else
                {
                    if (dynamicWebApiAttr != null)
                    {
                        ConfigureArea(controller, dynamicWebApiAttr);
                        ConfigureController(controller, dynamicWebApiAttr);
                        ConfigureDynamicWebApi(controller, dynamicWebApiAttr);
                    }
                }

            }
        }

        private void ConfigureArea(ControllerModel controller, DynamicWebApiAttribute attr)
        {
            if (attr == null)
                throw new ArgumentException(nameof(attr));

            if (!controller.RouteValues.ContainsKey("area"))
            {
                if (!string.IsNullOrEmpty(attr.Module))
                {
                    controller.RouteValues["area"] = attr.Module;
                }
                else if (!string.IsNullOrEmpty(AppConsts.DefaultAreaName))
                {
                    controller.RouteValues["area"] = AppConsts.DefaultAreaName;
                }
            }
        }

        private void ConfigureController(ControllerModel controller, DynamicWebApiAttribute attr)
        {
            if (attr == null)
                throw new ArgumentException(nameof(attr));

            if (!string.IsNullOrEmpty(attr.Name))
                controller.ControllerName = attr.Name;

            controller.ControllerName = controller.ControllerName.RemovePostFix(AppConsts.ControllerPostfixes.ToArray());

            if (AppConsts.SplitControllerCamelCase)
                controller.ControllerName = string.Join(AppConsts.SplitControllerCamelCaseSeparator, controller.ControllerName.SplitCamelCase());

            if (!string.IsNullOrEmpty(attr.Version))
                controller.ControllerName = $"{ controller.ControllerName }{AppConsts.VersionSeparator}{ attr.Version }";
        }

        private void ConfigureDynamicWebApi(ControllerModel controller, DynamicWebApiAttribute controllerAttr)
        {
            ConfigureApiExplorer(controller);
            ConfigureSelector(controller, controllerAttr);
            ConfigureParameters(controller);
        }


        private void ConfigureParameters(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    foreach (var para in action.Parameters)
                    {
                        if (para.BindingInfo != null)
                        {
                            continue;
                        }

                        if (!TypeHelper.IsPrimitiveExtendedIncludingNullable(para.ParameterInfo.ParameterType))
                        {
                            if (CanUseFormBodyBinding(action, para))
                            {
                                para.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                            }
                        }
                    }
            }
        }


        private bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
        {
            if (AppConsts.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
            {
                return false;
            }

            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints == null)
                {
                    continue;
                }

                foreach (var actionConstraint in selector.ActionConstraints)
                {

                    var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                    if (httpMethodActionConstraint == null)
                    {
                        continue;
                    }

                    if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        #region ConfigureApiExplorer

        private void ConfigureApiExplorer(ControllerModel controller)
        {
            if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            if (controller.ApiExplorer.IsVisible == null)
            {
                controller.ApiExplorer.IsVisible = true;
            }

            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    ConfigureApiExplorer(action);
            }
        }

        private void ConfigureApiExplorer(ActionModel action)
        {
            if (action.ApiExplorer.IsVisible == null)
            {
                action.ApiExplorer.IsVisible = true;
            }
        }

        #endregion
        /// <summary>
        /// //不映射指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool CheckNoMapMethod(ActionModel action)
        {
            bool isExist = false;
            var noMapMethod = ReflectionHelper.GetSingleAttributeOrDefault<NonDynamicMethodAttribute>(action.ActionMethod);

            if (noMapMethod != null)
            {
                action.ApiExplorer.IsVisible = false;//对应的Api不映射
                isExist = true;
            }

            return isExist;
        }
        private void ConfigureSelector(ControllerModel controller, DynamicWebApiAttribute controllerAttr)
        {
            if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
                return;

            var areaName = string.Empty;

            if (controllerAttr != null)
                areaName = controllerAttr.Module;

            if (AppConsts.IsAutoSortAction)
            {
                var dict = new Dictionary<string, List<ActionModel>>();
                dict["POST"] = new List<ActionModel>();
                dict["PUT"] = new List<ActionModel>();
                dict["DELETE"] = new List<ActionModel>();
                dict["GET"] = new List<ActionModel>();

                foreach (var action in controller.Actions)
                {
                    var verb = GetHttpVerb(action).ToUpper();

                    if (!CheckNoMapMethod(action))
                        ConfigureSelector(areaName, controller.ControllerName, action);

                    if (dict.ContainsKey(verb))
                        dict[verb].Add(action);
                }

                controller.Actions.Clear();

                foreach (var item in dict)
                {
                    foreach (var action in item.Value.OrderBy(c => c.ActionName))
                        controller.Actions.Add(action);
                }
            }
            else
            {
                foreach (var action in controller.Actions)
                {
                    if (!CheckNoMapMethod(action))
                        ConfigureSelector(areaName, controller.ControllerName, action);
                }
            }
        }

        private void ConfigureSelector(string areaName, string controllerName, ActionModel action)
        {

            var nonAttr = ReflectionHelper.GetSingleAttributeOrDefault<NonDynamicWebApiAttribute>(action.ActionMethod);

            if (nonAttr != null) return;

            if (action.Selectors.IsNullOrEmpty() || action.Selectors.Any(a => a.ActionConstraints.IsNullOrEmpty()))
            {
                if (!CheckNoMapMethod(action))
                    AddAppServiceSelector(areaName, controllerName, action);
            }
            else
            {
                NormalizeSelectorRoutes(areaName, controllerName, action);
            }
        }

        private void AddAppServiceSelector(string areaName, string controllerName, ActionModel action)
        {
            var verb = GetHttpVerb(action);

            action.ActionName = GetRestFulActionName(action.ActionName);

            var appServiceSelectorModel = action.Selectors[0];

            if (appServiceSelectorModel.AttributeRouteModel == null)
            {
                appServiceSelectorModel.AttributeRouteModel = CreateActionRouteModel(areaName, controllerName, action);
            }

            if (!appServiceSelectorModel.ActionConstraints.Any())
            {
                appServiceSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));
                switch (verb)
                {
                    case "GET":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpGetAttribute());
                        break;
                    case "POST":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpPostAttribute());
                        break;
                    case "PUT":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpPutAttribute());
                        break;
                    case "DELETE":
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpDeleteAttribute());
                        break;
                    default:
                        throw new Exception($"Unsupported http verb: {verb}.");
                }
            }


        }

        /// <summary>
        /// Processing action name
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        private static string GetRestFulActionName(string actionName)
        {
            // custom process action name
            var appConstsActionName = AppConsts.GetRestFulActionName?.Invoke(actionName);
            if (appConstsActionName != null)
                return appConstsActionName;

            // default process action name.

            // Remove Postfix
            actionName = actionName.RemovePostFix(AppConsts.ActionPostfixes.ToArray());

            // Remove Prefix
            if (AppConsts.IsRemoveVerbs)
            {
                var verbKey = actionName.GetPascalOrCamelCaseFirstWord().ToLower();

                if (AppConsts.HttpVerbs.ContainsKey(verbKey))
                {
                    if (actionName.Length == verbKey.Length)
                        actionName = "";
                    else
                        actionName = actionName.Substring(verbKey.Length);
                }
            }

            if (AppConsts.SplitActionCamelCase)
                actionName = string.Join(AppConsts.SplitActionCamelCaseSeparator, actionName.SplitCamelCase());

            return actionName;
        }

        private static void NormalizeSelectorRoutes(string areaName, string controllerName, ActionModel action)
        {
            action.ActionName = GetRestFulActionName(action.ActionName);

            foreach (var selector in action.Selectors)
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel == null ?
                     CreateActionRouteModel(areaName, controllerName, action) :
                     AttributeRouteModel.CombineAttributeRouteModel(CreateActionRouteModel(areaName, controllerName, action), selector.AttributeRouteModel);
            }
        }

        private static string GetHttpVerb(ActionModel action)
        {
            var getValueSuccess = AppConsts.AssemblyDynamicWebApiOptions
                .TryGetValue(action.Controller.ControllerType.Assembly, out AssemblyDynamicWebApiOptions assemblyDynamicWebApiOptions);
            if (getValueSuccess && !string.IsNullOrWhiteSpace(assemblyDynamicWebApiOptions?.HttpVerb))
            {
                return assemblyDynamicWebApiOptions.HttpVerb;
            }


            var verbKey = action.ActionName.GetPascalOrCamelCaseFirstWord().ToLower();

            var verb = AppConsts.HttpVerbs.ContainsKey(verbKey) ? AppConsts.HttpVerbs[verbKey] : AppConsts.DefaultHttpVerb;
            return verb;
        }

        private static string GetApiPreFix(ActionModel action)
        {
            var getValueSuccess = AppConsts.AssemblyDynamicWebApiOptions
                .TryGetValue(action.Controller.ControllerType.Assembly, out AssemblyDynamicWebApiOptions assemblyDynamicWebApiOptions);
            if (getValueSuccess && !string.IsNullOrWhiteSpace(assemblyDynamicWebApiOptions?.ApiPrefix))
            {
                return assemblyDynamicWebApiOptions.ApiPrefix;
            }

            return AppConsts.DefaultApiPreFix;
        }

        private static AttributeRouteModel CreateActionRouteModel(string areaName, string controllerName, ActionModel action)
        {
            var apiPreFix = GetApiPreFix(action);
            var routeStr = $"{apiPreFix}/{areaName}/{controllerName}/{action.ActionName}".Replace("//", "/");
            return new AttributeRouteModel(new RouteAttribute(routeStr));
        }
    }
}

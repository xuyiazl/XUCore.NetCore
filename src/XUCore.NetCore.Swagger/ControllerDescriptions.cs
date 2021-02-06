using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace XUCore.NetCore.Swagger
{
    /// <summary>
    /// 加入controller层描述
    /// </summary>
    public class ControllerDescriptions : IDocumentFilter
    {
        /// <summary>
        /// 匹配visual 生成的xml文件
        /// </summary>
        private const string MemberXPath = "/doc/members/member[@name='{0}']";
        /// <summary>
        /// 提取xml的标记点
        /// </summary>
        private const string SummaryTag = "summary";
        /// <summary>
        /// xml导航器
        /// </summary>
        private readonly XPathNavigator _xmlNavigator;
        /// <summary>
        /// 加入controller层描述
        /// </summary>
        /// <param name="apixmlpath"></param>
        public ControllerDescriptions(string apixmlpath)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(apixmlpath);
            _xmlNavigator = doc.CreateNavigator();
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var controllerNamesAndTypes = context.ApiDescriptions
                .Select(apiDesc => apiDesc.ActionDescriptor as ControllerActionDescriptor)
                .SkipWhile(actionDesc => actionDesc == null)
                .GroupBy(actionDesc => actionDesc.ControllerName)
                .ToDictionary(grp => grp.Key, grp => grp.Last().ControllerTypeInfo.AsType());

            //解析xml
            foreach (var nameAndType in controllerNamesAndTypes)
            {
                //获取成员名称，在xml中
                var memberName = XmlCommentsNodeNameHelper.GetMemberNameForType(nameAndType.Value);
                var typeNode = _xmlNavigator.SelectSingleNode(string.Format(MemberXPath, memberName));

                if (typeNode != null)
                {
                    var summaryNode = typeNode.SelectSingleNode(SummaryTag);
                    if (summaryNode != null)
                    {
                        if (swaggerDoc.Tags == null)
                            swaggerDoc.Tags = new List<OpenApiTag>();

                        swaggerDoc.Tags.Add(new OpenApiTag
                        {
                            Name = nameAndType.Key,
                            Description = $"<b>【{XmlCommentsTextHelper.Humanize(summaryNode.InnerXml)}】</b>"
                        });
                    }
                }
            }

        }
    }
}

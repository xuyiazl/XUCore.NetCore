using System.Collections.Generic;

namespace XUCore.WeChat.Apis.Message
{
    public class GetAllPrivateTemplateApiResult : ApiResultBase
    {


        /// <summary>
        /// 模板列表
        /// </summary>
        public List<TemplateInfo> TemplateList { get; set; }

    }
}
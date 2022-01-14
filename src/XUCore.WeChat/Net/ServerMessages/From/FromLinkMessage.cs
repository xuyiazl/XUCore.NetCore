// ======================================================================
//  
//          Copyright (C) 2018-2068 ����������Ϣ�Ƽ����޹�˾    
//          All rights reserved
//  
//          filename : FromLinkMessage.cs
//          description :
//  
//          created by codelove1314@live.cn at  2021-02-09 10:48:26
//          Blog��http://www.cnblogs.com/codelove/
//          GitHub �� https://github.com/xin-lai
//          Home��http://xin-lai.com
//  
// =======================================================================

namespace XUCore.WeChat.Net.ServerMessages.From
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class FromLinkMessage : FromMessageBase
    {
        /// <summary>
        /// Gets or sets the Title
        /// ��Ϣ����
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// ��Ϣ����
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Url
        /// ��Ϣ����
        /// </summary>
        public string Url { get; set; }
    }
}

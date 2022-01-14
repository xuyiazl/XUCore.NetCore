// ======================================================================
//  
//          Copyright (C) 2018-2068 ����������Ϣ�Ƽ����޹�˾    
//          All rights reserved
//  
//          filename : FromTextMessage.cs
//          description :
//  
//          created by codelove1314@live.cn at  2021-02-09 10:48:28
//          Blog��http://www.cnblogs.com/codelove/
//          GitHub �� https://github.com/xin-lai
//          Home��http://xin-lai.com
//  
// =======================================================================

namespace XUCore.WeChat.Net.ServerMessages.From
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// �����ı���Ϣ
    /// </summary>
    [XmlRoot("xml")]
    [Serializable]
    public class FromTextMessage : FromMessageBase
    {
        /// <summary>
        /// Gets or sets the Content
        /// �ı���Ϣ����
        /// </summary>
        public string Content { get; set; }
    }
}

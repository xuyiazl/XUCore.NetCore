﻿// ======================================================================
//  
//          Copyright (C) 2018-2068 湖南心莱信息科技有限公司    
//          All rights reserved
//  
//          filename : FromVideoMessage.cs
//          description :
//  
//          created by codelove1314@live.cn at  2021-02-09 10:48:28
//          Blog：http://www.cnblogs.com/codelove/
//          GitHub ： https://github.com/xin-lai
//          Home：http://xin-lai.com
//  
// =======================================================================

namespace XUCore.WeChat.AspNet.ServerMessages.From
{
    using System.Xml.Serialization;

    /// <summary>
    /// 视频消息
    /// </summary>
    public class FromVideoMessage : FromMessageBase
    {
        /// <summary>
        /// Gets or sets the ThumbMediaId
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// Gets or sets the MediaId
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        [XmlElement("MediaId")]
        public string MediaId { get; set; }
    }
}

// ======================================================================
//  
//          Copyright (C) 2018-2068 ����������Ϣ�Ƽ����޹�˾    
//          All rights reserved
//  
//          filename : ToNewsMessage.cs
//          description :
//  
//          created by codelove1314@live.cn at  2021-02-09 10:48:30
//          Blog��http://www.cnblogs.com/codelove/
//          GitHub �� https://github.com/xin-lai
//          Home��http://xin-lai.com
//  
// =======================================================================

namespace XUCore.WeChat.AspNet.ServerMessages.To
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// �ظ�ͼ����Ϣ
    /// </summary>
    [XmlRoot(ElementName = "xml")]
    public class ToNewsMessage : ToMessageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToNewsMessage"/> class.
        /// </summary>
        public ToNewsMessage()
        {
            Type = ToMessageTypes.news;
            Articles = new List<ArticleInfo>();
        }

        /// <summary>
        /// Gets or sets the ArticleCount
        /// ͼ����Ϣ����������Ϊ10������
        /// </summary>
        [XmlElement("ArticleCount")]
        public int ArticleCount { get; set; }

        /// <summary>
        /// Gets or sets the Articles
        /// ����ͼ����Ϣ��Ϣ��Ĭ�ϵ�һ��itemΪ��ͼ,ע�⣬���ͼ��������10���򽫻�����Ӧ
        /// </summary>
        [XmlArrayItem("Item")]
        public List<ArticleInfo> Articles { get; set; }

        /// <summary>
        /// Defines the <see cref="ArticleInfo" />
        /// </summary>
        [Serializable]
        public class ArticleInfo
        {
            /// <summary>
            /// Gets or sets the Title
            /// ͼ����Ϣ����
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// Gets or sets the Description
            /// ͼ����Ϣ����
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the PicUrl
            /// ͼƬ���ӣ�֧��JPG��PNG��ʽ���Ϻõ�Ч��Ϊ��ͼ360*200��Сͼ200*200
            /// </summary>
            public string PicUrl { get; set; }

            /// <summary>
            /// Gets or sets the Url
            /// ���ͼ����Ϣ��ת����
            /// </summary>
            public string Url { get; set; }
        }
    }
}

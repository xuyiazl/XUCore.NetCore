﻿using Newtonsoft.Json;

namespace XUCore.WeChat.Apis.Message
{
    /// <summary>
    /// Defines the <see cref="PreviewWXCardInput" />
    /// </summary>
    public class PreviewWXCardInput : PreviewInputBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewWXCardInput"/> class.
        /// </summary>
        public PreviewWXCardInput()
        {
            MessageType = MessageTypes.wxcard;
        }

        /// <summary>
        /// Gets or sets the WXCard
        /// </summary>
        [JsonProperty("wxcard")]
        public WXCardInfo WXCard { get; set; }

        /// <summary>
        /// Defines the <see cref="WXCardInfo" />
        /// </summary>
        public class WXCardInfo
        {
            /// <summary>
            /// Gets or sets the MediaId
            /// </summary>
            [JsonProperty("media_id")]
            public string MediaId { get; set; }
        }
    }
}
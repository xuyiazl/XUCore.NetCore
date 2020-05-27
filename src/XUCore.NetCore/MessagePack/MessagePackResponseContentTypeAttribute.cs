using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.MessagePack
{
    public class MessagePackResponseContentTypeAttribute : ProducesAttribute
    {
        public MessagePackResponseContentTypeAttribute() : base("application/json", "application/x-msgpack", "application/x-msgpack-jackson")
        {

        }

        public MessagePackResponseContentTypeAttribute(string contentType, params string[] additionalContentTypes) : base(contentType, additionalContentTypes)
        {

        }
    }
}

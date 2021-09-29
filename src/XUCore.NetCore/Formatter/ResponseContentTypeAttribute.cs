using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Formatter
{
    public class ResponseContentTypeAttribute : ProducesAttribute
    {
        public ResponseContentTypeAttribute() : base("application/json", "application/x-msgpack", "application/x-msgpack-jackson")
        {

        }

        public ResponseContentTypeAttribute(string contentType, params string[] additionalContentTypes) : base(contentType, additionalContentTypes)
        {

        }
    }
}

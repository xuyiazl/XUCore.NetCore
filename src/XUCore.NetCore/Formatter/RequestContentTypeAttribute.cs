using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Formatter
{
    public class RequestContentTypeAttribute : ConsumesAttribute
    {
        public RequestContentTypeAttribute() : base("application/json", "application/x-msgpack")
        {

        }

        public RequestContentTypeAttribute(string contentType, params string[] additionalContentTypes) : base(contentType, additionalContentTypes)
        {

        }
    }
}

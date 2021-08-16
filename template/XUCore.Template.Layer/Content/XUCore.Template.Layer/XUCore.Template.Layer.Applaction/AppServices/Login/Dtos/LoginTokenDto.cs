using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.Layer.Applaction.Login
{
    public class LoginTokenDto
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }
}

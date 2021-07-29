using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample1.Applaction.Login
{
    public class LoginTokenDto
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }
}

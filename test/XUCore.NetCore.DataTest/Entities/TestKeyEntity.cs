using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Ddd.Domain;

namespace XUCore.NetCore.DataTest.Entities
{
    public class TestKeyEntity : Entity<string>
    {
        public string Name { get; set; }
    }
}

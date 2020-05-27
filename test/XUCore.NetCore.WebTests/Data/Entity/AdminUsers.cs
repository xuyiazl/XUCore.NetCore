using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.WebTests.Data.Entity
{
    [MessagePackObject]
    public class AdminUsers : BaseEntity
    {
        [Key(1)]
        public string UserName { get; set; }
        [Key(2)]
        public string Mobile { get; set; }
        [Key(3)]
        public string Password { get; set; }
        [Key(4)]
        public string Name { get; set; }
        [Key(5)]
        public string Picture { get; set; }
        [Key(6)]
        public string Location { get; set; }
        [Key(7)]
        public string Position { get; set; }
        [Key(8)]
        public string Company { get; set; }
        [Key(9)]
        public int LoginCount { get; set; }
        [Key(10)]
        public DateTime LoginLastTime { get; set; }
        [Key(11)]
        public string LoginLastIp { get; set; }
        [Key(12)]
        public DateTime CreatedTime { get; set; }
        [Key(13)]
        public bool Status { get; set; }
    }
}

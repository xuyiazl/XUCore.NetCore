using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Mongo.Test
{
    [ModelProperty(ConnectionName = "testdb", TableName = "z_user_demo")]
    public class UserMongoModel : MongoBaseModel
    {
        public long AutoId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public IList<WorkModel> Works { get; set; }
    }

    public class WorkModel
    {
        public int Year { get; set; }
        public string CompanyName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Template.Layer.Core.Enums;

namespace XUCore.Template.Layer.DbService
{
    public abstract class CreateCommand : Command<bool>
    {

    }

    public abstract class UpdateCommand<TKey> : CommandId<bool, TKey>
    {

    }

    public abstract class ListCommand : CommandLimit<bool>
    {
        /// <summary>
        /// 排序，exp：“Id desc,CreatedAt desc”
        /// </summary>
        public string Orderby { get; set; }
    }

    public abstract class PageCommand : CommandPage<bool>
    {
        /// <summary>
        /// 排序，exp：“Id desc,CreatedAt desc”
        /// </summary>
        public string Orderby { get; set; }
    }
}

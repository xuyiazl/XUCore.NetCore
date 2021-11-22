using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 创建命令
    /// </summary>
    public abstract class CreateCommand : Command<bool>
    {

    }
    /// <summary>
    /// 更新命令
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class UpdateCommand<TKey> : CommandId<bool, TKey>
    {

    }
    /// <summary>
    /// 列表命令
    /// </summary>
    public abstract class ListCommand : CommandLimit<bool>
    {

    }
    /// <summary>
    /// 分页命令
    /// </summary>
    public abstract class PageCommand : CommandPage<bool>
    {

    }
}

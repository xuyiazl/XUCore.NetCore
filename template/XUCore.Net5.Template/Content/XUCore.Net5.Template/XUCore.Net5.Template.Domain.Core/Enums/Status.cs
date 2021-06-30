using System.ComponentModel;

namespace XUCore.Net5.Template.Domain.Core
{
    public enum Status
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        [Description("默认状态")]
        Default = 0,
        /// <summary>
        /// 正常显示
        /// </summary>
        [Description("正常显示")]
        Show = 1,
        /// <summary>
        /// 数据下架（隐藏）
        /// </summary>
        [Description("数据下架")]
        SoldOut = 2,
        /// <summary>
        /// 数据已被删除，非物理删除（回收站）
        /// </summary>
        [Description("回收站")]
        Trash = 3
    }
}

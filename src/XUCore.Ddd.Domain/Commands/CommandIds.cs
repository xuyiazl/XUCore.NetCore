using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using XUCore.Ddd.Domain.Events;

namespace XUCore.Ddd.Domain.Commands
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class CommandIds<TResponse> : Command<TResponse>
    {
        /// <summary>
        /// 主键Id集合
        /// </summary>
        public long[] Ids { get; set; }
        /// <summary>
        /// 抽象命令基类
        /// </summary>
        protected CommandIds() : base()
        {

        }
    }
}

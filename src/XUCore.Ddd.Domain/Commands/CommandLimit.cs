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
    public abstract class CommandLimit<TResponse> : Command<TResponse>
    {
        /// <summary>
        /// 记录数
        /// </summary>
        public int Limit { get; set; }
        /// <summary>
        /// 抽象命令基类
        /// </summary>
        protected CommandLimit() : base()
        {

        }
    }
}

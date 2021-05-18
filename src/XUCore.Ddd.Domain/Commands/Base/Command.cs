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
    //public abstract class Command<TResponse> : Message, IRequest<TResponse>
    public abstract class Command<TResponse> : IRequest<TResponse>
    {
        ///// <summary>
        ///// 时间戳
        ///// </summary>
        //public DateTime Timestamp { get; private set; }
        /// <summary>
        /// 命令验证
        /// </summary>
        protected ValidationResult ValidationResult
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        protected Command()
        {
            //Timestamp = DateTime.Now;
            ValidationResult = new ValidationResult();
        }
        /// <summary>
        /// 获取验证的错误消息
        /// </summary>
        /// <returns></returns>
        public virtual IList<ValidationFailure> GetErrors() => ValidationResult.Errors;
        /// <summary>
        /// 获取验证的错误消息
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public virtual string GetErrors(string separator) => ValidationResult.ToString(separator);
        /// <summary>
        /// 是否验证通过
        /// </summary>
        /// <returns></returns>
        public virtual bool IsVaild() => ValidationResult.IsValid;
    }
}

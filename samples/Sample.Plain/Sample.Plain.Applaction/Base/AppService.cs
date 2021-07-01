﻿using System;
using XUCore.NetCore;
using Sample.Plain.Core;

namespace Sample.Plain.Applaction
{
    public class AppService : IAppService
    {
        public AppService()
        {
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Success<T>(string subCode, string message, T data = default) =>
            new Result<T>()
            {
                Code = 0,
                SubCode = subCode,
                Message = message,
                Data = data,
                ElapsedTime = -1,
                OperationTime = DateTime.Now
            };
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Success<T>(SubCode subCode, T data = default)
        {
            (var code, var message) = SubCodeMessage.Message(subCode);

            return new Result<T>()
            {
                Code = 0,
                SubCode = code,
                Message = message,
                Data = data,
                ElapsedTime = -1,
                OperationTime = DateTime.Now
            };
        }
        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Success<T>(SubCode subCode, string message, T data = default)
        {
            (var code, _) = SubCodeMessage.Message(subCode);

            return new Result<T>()
            {
                Code = 0,
                SubCode = code,
                Message = message,
                Data = data,
                ElapsedTime = -1,
                OperationTime = DateTime.Now
            };
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Fail<T>(string subCode, string message, T data = default) =>
             new Result<T>()
             {
                 Code = 0,
                 SubCode = subCode,
                 Message = message,
                 Data = data,
                 ElapsedTime = -1,
                 OperationTime = DateTime.Now
             };
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Fail<T>(SubCode subCode, string message, T data = default)
        {
            (var code, _) = SubCodeMessage.Message(subCode);

            return new Result<T>()
            {
                Code = 0,
                SubCode = code,
                Message = message,
                Data = data,
                ElapsedTime = -1,
                OperationTime = DateTime.Now
            };
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Fail<T>(SubCode subCode, T data = default)
        {
            (var code, var message) = SubCodeMessage.Message(subCode);

            return new Result<T>()
            {
                Code = 0,
                SubCode = code,
                Message = message,
                Data = data,
                ElapsedTime = -1,
                OperationTime = DateTime.Now
            };
        }
    }
}
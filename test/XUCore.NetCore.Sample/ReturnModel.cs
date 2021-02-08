namespace XUCore.NetCore.Sample
{
    /// <summary>
    /// 返回结构体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReturnModel<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 业务状态码
        /// </summary>
        public string subCode { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 请求耗时
        /// </summary>
        public long requestTime { get; set; } = -1;
        /// <summary>
        /// 数据
        /// </summary>
        public T bodyMessage { get; set; }
    }
}
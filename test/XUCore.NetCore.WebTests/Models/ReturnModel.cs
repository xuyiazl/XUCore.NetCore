namespace XUCore.WebTests
{
    /// <summary>
    /// 返回数据结构
    /// </summary>
    public class ReturnModel
    {
        private string _code = "-1";
        private string _subCode = "0";
        private long _requestLine = -1;
        private string _message = "service exception";
        private object _bodyMessage = "";

        /// <summary>
        ///
        /// </summary>
        public ReturnModel()
        {
            code = _code;
            subCode = _subCode;
            message = _message;
        }

        /// <summary>
        ///
        /// </summary>
        public ReturnModel(bool boolCode)
        {
            code = "0";
            subCode = _subCode;
            message = "";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        public ReturnModel(string msg)
        {
            code = _code;
            subCode = _subCode;
            message = msg;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="bodyMsg"></param>
        public ReturnModel(string msg, string bodyMsg)
        {
            message = msg;
            bodyMessage = bodyMsg;
        }

        /// <summary>
        /// 请求状态码：0 成功，-1 失败
        /// </summary>
        public string code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 业务状态码
        /// </summary>
        public string subCode
        {
            get { return _subCode; }
            set { _subCode = value; }
        }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long requestLine
        {
            get { return _requestLine; }
            set { _requestLine = value; }
        }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// json数据
        /// </summary>
        public object bodyMessage
        {
            get { return _bodyMessage; }
            set { _bodyMessage = value; }
        }
    }
}
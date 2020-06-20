using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Develops;

namespace XUCore.NetCore.Sample
{
    public class RetrySample : ISample
    {
        public void Run()
        {
            var model = 1;

            Retry.Run(model,
                (data, ndx) =>
                {
                    throw new Exception($"第{ndx}次失败异常测试");
                },
                (ndx, error) =>
                {
                    Console.WriteLine(error.Message);
                });

            Retry.Run(model,
                RetryAdapter.Create().Runs(5).Wait(1000),
                (data, ndx) =>
                {
                    throw new Exception($"第{ndx}次失败异常测试");
                },
                (ndx, error) =>
                {
                    Console.WriteLine(error.Message);
                });
        }
    }
}
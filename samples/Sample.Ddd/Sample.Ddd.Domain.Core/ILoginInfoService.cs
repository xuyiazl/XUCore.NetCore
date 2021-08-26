﻿namespace Sample.Ddd.Domain.Core
{
    public interface ILoginInfoService
    {
        bool IsAuthenticated { get; }

        string UserId { get; }

        string UserName { get; }
    }
}
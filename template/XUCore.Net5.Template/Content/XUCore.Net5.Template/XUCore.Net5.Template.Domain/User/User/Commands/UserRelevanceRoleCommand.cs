﻿using AutoMapper;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Auth;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.Data;

namespace XUCore.Net5.Template.Domain.User.User
{
    /// <summary>
    /// 关联角色命令
    /// </summary>
    public class UserRelevanceRoleCommand : Command<int>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// 角色id集合
        /// </summary>
        public string[] RoleIds { get; set; }

        public class Validator : CommandValidator<UserRelevanceRoleCommand>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("UserId");
            }
        }

        public class Handler : CommandHandler<UserRelevanceRoleCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [UnitOfWork(typeof(INigelDbContext))]
            public override async Task<int> Handle(UserRelevanceRoleCommand request, CancellationToken cancellationToken)
            {
                //先清空用户的角色，确保没有冗余的数据
                await db.DeleteAsync<UserRoleEntity>(c => c.UserId == request.UserId);

                var userRoles = Array.ConvertAll(request.RoleIds, roleid => new UserRoleEntity
                {
                    RoleId = roleid,
                    UserId = request.UserId
                });

                //添加角色
                if (userRoles.Length > 0)
                    return await db.AddAsync(userRoles);

                return 1;
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XUCore.NetCore.ApiTests.Dtos;
using XUCore.NetCore.DynamicWebApi;
using XUCore.NetCore.Swagger;

namespace XUCore.NetCore.ApiTests.Dynamic
{
    [DynamicWebApi]
    [ApiExplorerSettings(GroupName = "test")]
    public class GoodAppleAppService : IDynamicWebApi
    {
        private static readonly Dictionary<int, string> Apples = new Dictionary<int, string>()
        {
            [1] = "Big Apple",
            [2] = "Small Apple"
        };
        public async Task CreateCAppleAsync(UpdateAppleDto dto)
        {
            await Task.Run(() =>
            {
                if (Apples.ContainsKey(dto.Id))
                {
                    Apples[dto.Id] = dto.Name;
                }
            });

        }
        public async Task UpdateBAppleAsync(UpdateAppleDto dto)
        {
            await Task.Run(() =>
            {
                if (Apples.ContainsKey(dto.Id))
                {
                    Apples[dto.Id] = dto.Name;
                }
            });

        }
        public async Task UpdateAppleAsync(UpdateAppleDto dto)
        {
            await Task.Run(() =>
            {
                if (Apples.ContainsKey(dto.Id))
                {
                    Apples[dto.Id] = dto.Name;
                }
            });

        }
        [AllowAnonymous]
        [NonDynamicMethod]
        public async Task UpdateAppleInfoIsGoodAsync(UpdateAppleDto dto)
        {
            await Task.Run(() =>
            {
                if (Apples.ContainsKey(dto.Id))
                {
                    Apples[dto.Id] = dto.Name;
                }
            });

        }

        /// <summary>
        /// Get An Apple.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        //[HiddenApi]
        public string Get(int id)
        {
            if (Apples.ContainsKey(id))
            {
                return Apples[id];
            }
            else
            {
                return "No Apple!";
            }
        }
        [HttpGet("{id:int}")]
        [NonDynamicMethod]
        public string GetById(int id)
        {
            if (Apples.ContainsKey(id))
            {
                return Apples[id];
            }
            else
            {
                return "No Apple!";
            }
        }

        /// <summary>
        /// Get  All Apple Async.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetAllAsync()
        {
            return Apples.Values;
        }

        /// <summary>
        /// Get All Apple.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Get()
        {
            return Apples.Values;
        }
        /// <summary>
        /// Get All Apple.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Create()
        {
            return Apples.Values;
        }

        /// <summary>
        /// Get All Apple.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetBigApple()
        {
            return Apples.Values;
        }

        /// <summary>
        /// Update Apple
        /// </summary>
        /// <param name="dto"></param>
        public void Update(UpdateAppleDto dto)
        {
            if (Apples.ContainsKey(dto.Id))
            {
                Apples[dto.Id] = dto.Name;
            }
        }

        /// <summary>
        /// Delete Apple
        /// </summary>
        /// <param name="id">Apple Id</param>
        public void Delete(int id)
        {
            if (Apples.ContainsKey(id))
            {
                Apples.Remove(id);
            }
        }
    }
}

﻿using System.IO;
using XUCore.Randoms;

namespace XUCore.Files.Paths
{
    /// <summary>
    /// 默认路径生成器
    /// </summary>
    public class DefaultPathGenerator : PathGeneratorBase
    {
        /// <summary>
        /// 基路径
        /// </summary>
        private readonly IBasePath _basePath;

        /// <summary>
        /// 初始化一个<see cref="DefaultPathGenerator"/>类型的实例
        /// </summary>
        /// <param name="basePath">基路径</param>
        /// <param name="randomGenerator">随机数生成器</param>
        public DefaultPathGenerator(IBasePath basePath, IRandomGenerator randomGenerator) : base(randomGenerator)
        {
            _basePath = basePath;
        }

        /// <summary>
        /// 创建完整路径
        /// </summary>
        /// <param name="fileName">被处理过的安全有效的文件名</param>
        /// <returns></returns>
        protected override string GeneratePath(string fileName)
        {
            return Path.Combine(_basePath.GetPath(), fileName);
        }
    }
}
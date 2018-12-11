using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Abp.Extensions;
using Abp.Reflection.Extensions;
using System.IO;

namespace Yoyo.AzureAi.Configuration
{
    public static class AppConfigurations
    {
        private static readonly ConcurrentDictionary<string, IConfigurationRoot> _configurationCache;

        static AppConfigurations()
        {
            _configurationCache = new ConcurrentDictionary<string, IConfigurationRoot>();
        }

        /// <summary>
        /// 获取appsettings
        /// </summary>
        /// <param name="path"></param>
        /// <param name="environmentName"></param>
        /// <param name="addUserSecrets"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetAppsettings(string path, string environmentName = null, bool addUserSecrets = false)
        {
            string fileName = "appsettings";

            var cacheKey = path + "#" + fileName + "#" + environmentName + "#" + addUserSecrets;
            return _configurationCache.GetOrAdd(
                cacheKey,
                _ => BuildConfiguration(path, fileName, environmentName, addUserSecrets)
            );
        }

        /// <summary>
        /// 获取其它配置文件的内容
        /// </summary>
        /// <param name="path">目录</param>
        /// <param name="fileName">文件名</param>
        /// <param name="ex">文件扩展名</param>
        /// <param name="environmentName">环境变量</param>
        /// <returns>读取到的结果</returns>
        public static string GetOtherConfigContent(string path, string fileName, string ex = "json", string environmentName = null)
        {
            var configFilePath = string.Empty;

            if (!environmentName.IsNullOrWhiteSpace())
            {
                configFilePath = Path.Combine(path, $"{fileName}.{environmentName}.{ex}");
            }
            else
            {
                configFilePath = Path.Combine(path, $"{fileName}.{ex}");
            }


            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException($"目录 {path} 中未找到文件 {path}.{ex}");
            }


            return File.ReadAllText(configFilePath, System.Text.Encoding.UTF8);

        }
        private static IConfigurationRoot BuildConfiguration(string path, string fileName, string environmentName = null, bool addUserSecrets = false)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile($"{fileName}.json", optional: true, reloadOnChange: true);

            if (!environmentName.IsNullOrWhiteSpace())
            {
                builder = builder.AddJsonFile($"{fileName}.{environmentName}.json", optional: true);
            }

            builder = builder.AddEnvironmentVariables();

            if (addUserSecrets)
            {
                builder.AddUserSecrets(typeof(AppConfigurations).GetAssembly());
            }

            return builder.Build();
        }
    }
}

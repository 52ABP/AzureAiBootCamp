using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Abp.Extensions;
using Abp.Reflection.Extensions;

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
        /// 获取azuresettings
        /// </summary>
        /// <param name="path"></param>
        /// <param name="environmentName"></param>
        /// <param name="addUserSecrets"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetAzuresettings(string path, string environmentName = null, bool addUserSecrets = false)
        {
            string fileName = "azuresettings";

            var cacheKey = path + "#" + fileName + "#" + environmentName + "#" + addUserSecrets;
            return _configurationCache.GetOrAdd(
                cacheKey,
                _ => BuildConfiguration(path, fileName, environmentName, addUserSecrets)
            );
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

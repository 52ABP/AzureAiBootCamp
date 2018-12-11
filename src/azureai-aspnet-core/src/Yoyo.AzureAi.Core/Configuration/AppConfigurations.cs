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
        /// ��ȡappsettings
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
        /// ��ȡ���������ļ�������
        /// </summary>
        /// <param name="path">Ŀ¼</param>
        /// <param name="fileName">�ļ���</param>
        /// <param name="ex">�ļ���չ��</param>
        /// <param name="environmentName">��������</param>
        /// <returns>��ȡ���Ľ��</returns>
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
                throw new FileNotFoundException($"Ŀ¼ {path} ��δ�ҵ��ļ� {path}.{ex}");
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

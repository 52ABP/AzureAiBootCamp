using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Yoyo.AzureAi.Localization
{
    public static class AzureAiLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(AzureAiConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(AzureAiLocalizationConfigurer).GetAssembly(),
                        "Yoyo.AzureAi.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}

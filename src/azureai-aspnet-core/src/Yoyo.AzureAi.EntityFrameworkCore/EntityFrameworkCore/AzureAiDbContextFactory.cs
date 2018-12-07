using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Yoyo.AzureAi.Configuration;
using Yoyo.AzureAi.Web;

namespace Yoyo.AzureAi.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class AzureAiDbContextFactory : IDesignTimeDbContextFactory<AzureAiDbContext>
    {
        public AzureAiDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AzureAiDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            AzureAiDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AzureAiConsts.ConnectionStringName));

            return new AzureAiDbContext(builder.Options);
        }
    }
}

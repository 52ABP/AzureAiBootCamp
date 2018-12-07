using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.AzureAi.EntityFrameworkCore
{
    public static class AzureAiDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AzureAiDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AzureAiDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}

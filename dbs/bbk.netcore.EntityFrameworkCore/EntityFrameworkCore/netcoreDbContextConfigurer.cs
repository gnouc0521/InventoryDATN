using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace bbk.netcore.EntityFrameworkCore
{
    public static class netcoreDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<netcoreDbContext> builder, string connectionString)
        {
            builder.UseNpgsql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<netcoreDbContext> builder, DbConnection connection)
        {
            builder.UseNpgsql(connection);
        }
    }
}

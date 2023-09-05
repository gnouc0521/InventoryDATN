using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using bbk.netcore.Configuration;
using bbk.netcore.Web;

namespace bbk.netcore.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class netcoreDbContextFactory : IDesignTimeDbContextFactory<netcoreDbContext>
    {
        public netcoreDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<netcoreDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            netcoreDbContextConfigurer.Configure(builder, configuration.GetConnectionString(netcoreConsts.ConnectionStringName));

            return new netcoreDbContext(builder.Options);
        }
    }
}

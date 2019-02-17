using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace MitchRankChecker.EntityFramework
{
    /// <summary>
    /// <para>
    /// DbContext Factory used to created our DbContext
    /// during design time, i.e. when we create our
    /// migrations via:
    ///   dotnet ef migrations add :MigrationName:
    /// </para>
    /// <para>
    /// Without this class, we won't be able to
    /// create migrations from this Class Library
    /// project, and may give the error:
    ///   Unable to create an object of type :DbContext:.
    ///   For the different patterns supported at design
    ///   time, see https://go.microsoft.com/fwlink/?linkid=851728
    /// </para>
    /// </summary>
    /// <typeparam name="RankCheckerDbContext"></typeparam>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RankCheckerDbContext>
    {
        public RankCheckerDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Data Source=./RankChecker.sqlite";

            var builder = new DbContextOptionsBuilder<RankCheckerDbContext>();

            builder.UseSqlite(connectionString);

            return new RankCheckerDbContext(builder.Options);
        }
    }
}
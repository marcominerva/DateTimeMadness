using Microsoft.EntityFrameworkCore;

namespace DateTimeMadness.DataAccessLayer;

public class DataContext : DbContext
{
    public DbSet<Session> Sessions { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries().ToList();
        foreach (var entry in entries)
        {
            foreach (var property in entry.Properties.Where(p => p.IsModified))
            {

            }
        }

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

public class Session
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime ProposedDate { get; set; }
}
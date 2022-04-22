using Microsoft.EntityFrameworkCore;

namespace DateTimeMadness.DataAccessLayer;

public class DataContext : DbContext
{
    public DbSet<Session> Sessions { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}

public class Session
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime ProposedDate { get; set; }
}
using Microsoft.EntityFrameworkCore;
using UserConnectionService.Data.Entities;

namespace UserConnectionService.Data;

public class UserMonitoringContext : DbContext
{
    public UserMonitoringContext(DbContextOptions<UserMonitoringContext> options) : base(options) { }

    public DbSet<UserConnectionEvent> UserConenctionEvents { get; set; }

    public DbSet<ErrorEvent> ErrorEvents { get; set; }
}

using Microsoft.EntityFrameworkCore;
using UserConnectionService.Data.Entities;

namespace UserConnectionService.Data;

public class UserMonitoringContext : DbContext
{    
    public DbSet<UserConnectionEvent> UserConenctionEvents { get; set; }

    public DbSet<ErrorEvent> ErrorEvents { get; set; }
}

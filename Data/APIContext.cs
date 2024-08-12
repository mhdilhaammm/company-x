using Microsoft.EntityFrameworkCore;
using Company_X.Models;

namespace Company_X.Data
{
    public class APIContext : DbContext
    {
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public APIContext(DbContextOptions<APIContext> opt) : base(opt) { }
    }
}

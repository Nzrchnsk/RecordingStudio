using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecordingStudio.Models
{
    public class RecordingStudioDbContext: IdentityDbContext<User, IdentityRole<int>,int>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Studio> Studios  { get; set; }
        public RecordingStudioDbContext(DbContextOptions<RecordingStudioDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
    }
}
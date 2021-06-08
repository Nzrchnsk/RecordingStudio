using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecordingStudio.Models
{
    public class RecordingStudioDbContext: IdentityDbContext<User, IdentityRole<int>,int>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Studio> Studios  { get; set; }
        public RecordingStudioDbContext(DbContextOptions<RecordingStudioDbContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
            if (!Studios.Any())
            {
                Studios.Add(new Studio
                {
                    Name = "Студия 1",
                    Description = "фывфывфывфывфыв",
                    Address = "Улица Ленина, дом короля"
                });
                Studios.Add(new Studio
                {
                    Name = "Студия 2",
                    Description = "ывпщмлыоващплыоавщ  werwer",
                    Address = "йцуйцуйцу"
                });
                SaveChanges();
            }
        }
    }
}
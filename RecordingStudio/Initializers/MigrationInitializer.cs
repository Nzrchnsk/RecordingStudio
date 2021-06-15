using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecordingStudio.Models;

namespace RecordingStudio.Initializers
{
    public class MigrationInitializer
    {
        private readonly RecordingStudioDbContext _applicationDbContext;

        public MigrationInitializer(RecordingStudioDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task Run()
        {
            await _applicationDbContext.Database.MigrateAsync();
            if (!_applicationDbContext.Studios.Any())
            {
                _applicationDbContext.Studios.Add(new Studio
                {
                    Name = "Студия 1",
                    Description = "Супер для вокала",
                    Address = "Улица Пушкина, дом 45",
                    Price = 4321
                });
                _applicationDbContext.Studios.Add(new Studio
                {
                    Name = "Студия 2",
                    Description = "Супер для инструментов",
                    Address = "Улица Мира, дом 88 ",
                    Price = 1000.1
                });
                await _applicationDbContext.SaveChangesAsync();
            }
        }
    }
}
using Event_Registration_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_Registration_System.Data
{
    public class EventDbContext: DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
            
        }
        public DbSet<Event> events { get; set; }
        public DbSet<Registration> registrations { get; set; }

    }
}

using APBD_Cw7_S29551.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_Cw7_S29551.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
    }
}

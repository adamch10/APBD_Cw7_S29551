using APBD_Cw7_S29551.Data;
using APBD_Cw7_S29551.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_Cw7_S29551.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync() => await _context.Tickets.ToListAsync();

        public async Task<Ticket?> GetByIdAsync(int id) =>
            await _context.Tickets.Include(t => t.Comments).FirstOrDefaultAsync(t => t.Id == id);

        public async Task CreateTicketWithCommentAsync(Ticket ticket, TicketComment comment)
        {
            // Gwarancja atomowości: zapis zgłoszenia i komentarza w jednej transakcji
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                comment.TicketId = ticket.Id;
                _context.TicketComments.Add(comment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateStatusAsync(int id, TicketStatus status)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                ticket.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}

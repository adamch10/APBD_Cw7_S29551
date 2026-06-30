using APBD_Cw7_S29551.Models;

namespace APBD_Cw7_S29551.Repositories
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(int id);
        Task CreateTicketWithCommentAsync(Ticket ticket, TicketComment comment);
        Task UpdateStatusAsync(int id, TicketStatus status);
    }
}

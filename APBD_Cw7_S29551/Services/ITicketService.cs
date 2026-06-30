using APBD_Cw7_S29551.Models;

namespace APBD_Cw7_S29551.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketDetailsAsync(int id);
        Task CreateTicketAsync(string title, string? description, string author, string commentContent);
        Task CloseTicketAsync(int id);
    }
}

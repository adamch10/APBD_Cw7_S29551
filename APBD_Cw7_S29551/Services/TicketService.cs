using APBD_Cw7_S29551.Models;
using APBD_Cw7_S29551.Repositories;

namespace APBD_Cw7_S29551.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;

        public TicketService(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync() => await _repository.GetAllAsync();

        public async Task<Ticket?> GetTicketDetailsAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task CreateTicketAsync(string title, string? description, string author, string commentContent)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(commentContent))
            {
                throw new ArgumentException("Tytuł oraz pierwszy komentarz nie mogą być puste.");
            }

            var ticket = new Ticket
            {
                Title = title,
                Description = description,
                Status = TicketStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            var comment = new TicketComment
            {
                Author = author,
                Content = commentContent,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateTicketWithCommentAsync(ticket, comment);
        }

        public async Task CloseTicketAsync(int id)
        {
            await _repository.UpdateStatusAsync(id, TicketStatus.Closed);
        }
    }
}

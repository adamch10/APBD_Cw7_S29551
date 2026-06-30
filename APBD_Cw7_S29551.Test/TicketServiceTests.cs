using APBD_Cw7_S29551.Models;
using APBD_Cw7_S29551.Repositories;
using APBD_Cw7_S29551.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace APBD_Cw7_S29551.Test
{
    public class TicketServiceTests
    {
        private class FakeTicketRepository : ITicketRepository
        {
            public List<Ticket> Tickets = new();
            public List<TicketComment> Comments = new();

            public Task CreateTicketWithCommentAsync(Ticket ticket, TicketComment comment)
            {
                ticket.Id = Tickets.Count + 1;
                Tickets.Add(ticket);
                comment.TicketId = ticket.Id;
                Comments.Add(comment);
                return Task.CompletedTask;
            }

            public Task<IEnumerable<Ticket>> GetAllAsync() => Task.FromResult(Tickets.AsEnumerable());
            public Task<Ticket?> GetByIdAsync(int id) => Task.FromResult(Tickets.FirstOrDefault(t => t.Id == id));

            public Task UpdateStatusAsync(int id, TicketStatus status)
            {
                var ticket = Tickets.FirstOrDefault(t => t.Id == id);
                if (ticket != null) ticket.Status = status;
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task CreateTicketAsync_ValidData_CreatesTicketAndComment()
        {
            // Arrange
            var repo = new FakeTicketRepository();
            var service = new TicketService(repo);

            // Act
            await service.CreateTicketAsync("Problem z myszką", "Nie działa scroll", "JanK", "Sprawdziłem na innym PC");

            // Assert
            Assert.Single(repo.Tickets);
            Assert.Single(repo.Comments);
            Assert.Equal(TicketStatus.Open, repo.Tickets[0].Status);
        }

        [Fact]
        public async Task CreateTicketAsync_EmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var repo = new FakeTicketRepository();
            var service = new TicketService(repo);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateTicketAsync("", "Opis", "JanK", "Komentarz"));
        }

        [Fact]
        public async Task CloseTicketAsync_ChangesStatusToClosed()
        {
            // Arrange
            var repo = new FakeTicketRepository();
            var service = new TicketService(repo);
            repo.Tickets.Add(new Ticket { Id = 1, Status = TicketStatus.Open });

            // Act
            await service.CloseTicketAsync(1);

            // Assert
            Assert.Equal(TicketStatus.Closed, repo.Tickets[0].Status);
        }
    }
}

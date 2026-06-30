using APBD_Cw7_S29551.Models;
using APBD_Cw7_S29551.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Cw7_S29551.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return View(tickets);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _ticketService.CreateTicketAsync(model.Title, model.Description, model.Author, model.InitialComment);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetTicketDetailsAsync(id);
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Close(int id)
        {
            await _ticketService.CloseTicketAsync(id);
            return RedirectToAction(nameof(Details), new { id });
        }
    }

}
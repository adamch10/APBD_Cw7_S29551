using System.ComponentModel.DataAnnotations;

namespace APBD_Cw7_S29551.Models
{
    public class CreateTicketViewModel
    {
        [Required(ErrorMessage = "Tytuł jest wymagany.")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Required(ErrorMessage = "Autor jest wymagany.")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pierwszy komentarz jest wymagany.")]
        public string InitialComment { get; set; } = string.Empty;
    }
}

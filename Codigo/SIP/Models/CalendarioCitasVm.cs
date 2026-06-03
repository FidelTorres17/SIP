using Microsoft.AspNetCore.Mvc.Rendering;

namespace SIP.Models
{
    public class CalendarioCitasVm
    {
        public bool EsAdmin { get; set; }
        public List<SelectListItem> Terapeutas { get; set; } = new();
    }
}

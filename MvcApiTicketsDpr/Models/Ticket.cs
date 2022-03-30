using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApiTicketsDpr.Models
{

    public class Ticket
    {

        public int IdTicket { get; set; }

        public int IdUsuario { get; set; }

        public String Importe { get; set; }

        public String Producto { get; set; }

        public String Filename { get; set; }

        public String StoragePath { get; set; }

        public DateTime Fecha { get; set; }

    }
}

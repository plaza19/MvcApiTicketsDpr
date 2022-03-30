using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApiTicketsDpr.Models
{
    public class UsuarioTicket
    {
       
        public int Idusuario { get; set; }
        
        public String Nombre { get; set; }
        
        public String Apellidos { get; set; }
      
        public String Email { get; set; }
    
        public String Username { get; set; }
     
        public String Password { get; set; }
    }
}

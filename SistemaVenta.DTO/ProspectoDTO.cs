using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class ProspectoDTO
    {
        public int IdProspecto { get; set; }

        public string? Nombre { get; set; }

        public string? Fachadaimg { get; set; }

        public string? Url { get; set; }

        public string? Direccion { get; set; }

        public string? Contacto { get; set; }

        public string? RazonSocial { get; set; }

        public int? Idauditor { get; set; }

        public string? Detalle { get; set; }

        public int? EsActivo { get; set; }



    }
}

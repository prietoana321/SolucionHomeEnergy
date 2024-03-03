using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class ImagenDownloadDTO
    {
        public int IdImagen { get; set; }
        public string? Nombre { get; set; }

        public int IdFile { get; set; }
    }
}

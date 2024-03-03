using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SistemaVenta.Models
{
    public partial class FileData
    {
        public int IdFile { get; set; }

        public string? Name { get; set; }

        public string? Extension { get; set; }

        public string? MimeType { get; set; }

        public string? Path { get; set; }
        public int? IdImagen { get; set; }

        public virtual Imagen? IdImagenNavigation { get; set; }
    }
}

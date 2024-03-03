using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.Models
{
    public partial class Imagen
    {
        public int IdImagen { get; set; }

        public string? Nombre { get; set; }

        public virtual ICollection<FileData> FileData { get; } = new List<FileData>();
    }
}

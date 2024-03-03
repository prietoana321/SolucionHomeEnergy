using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class FileDataDTO
    {
        public int IdFile { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Image { get; set; }
    }
}

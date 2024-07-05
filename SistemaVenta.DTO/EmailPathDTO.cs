using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DTO
{
    public class EmailPathDTO
    {
        public string Para { get; set; } = string.Empty;

        public string Asunto { get; set; } = string.Empty;

        public string Contenido { get; set; } = string.Empty;

        public string? PathAdjunto { get; set; } = string.Empty;
    }
}

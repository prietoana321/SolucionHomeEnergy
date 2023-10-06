using System;
using System.Collections.Generic;

namespace SistemaVenta.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string? Nombre { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; } = new List<DetalleVenta>();
}

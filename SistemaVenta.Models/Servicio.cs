using System;
using System.Collections.Generic;

namespace SistemaVenta.Models;

public partial class Servicio
{
    public int IdServicio { get; set; }

    public string? Nombre { get; set; }

    public int? IdCategoria { get; set; }

    public decimal? Precio { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; } = new List<DetalleVenta>();

    public virtual Categoria? IdCategoriaNavigation { get; set; }
}

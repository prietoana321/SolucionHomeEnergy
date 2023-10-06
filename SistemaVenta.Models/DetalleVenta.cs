using System;
using System.Collections.Generic;

namespace SistemaVenta.Models;

public partial class DetalleVenta
{
    public int IdDetalleVenta { get; set; }

    public int? IdVenta { get; set; }

    public int? IdServicio { get; set; }

    public int? IdEstado { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public decimal? Total { get; set; }

    public virtual Estado? IdEstadoNavigation { get; set; }

    public virtual Servicio? IdServicioNavigation { get; set; }

    public virtual Venta? IdVentaNavigation { get; set; }
}

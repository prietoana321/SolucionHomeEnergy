using System;
using System.Collections.Generic;

namespace SistemaVenta.Models;

public partial class ClienteUsuario
{
    public int IdClienteUsuario { get; set; }

    public int? IdCliente { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}

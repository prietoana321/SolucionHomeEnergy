using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DLL.Servicios.Contrato
{
    public interface IProspectoService
    {
        Task<List<ProspectoDTO>> Lista();
        Task<ProspectoDTO> Crear(ProspectoDTO modelo);


        Task<bool> Editar(ProspectoDTO modelo);
        Task<bool> Eliminar(int id);
    }
}

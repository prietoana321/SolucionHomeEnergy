using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaVenta.DTO;

namespace SistemaVenta.DLL.Servicios.Contrato
{
   public interface IServicioService
    {
        Task<List<ServicioDTO>> Lista();
        Task<ServicioDTO> Crear(ServicioDTO modelo);
        Task<bool> Editar(ServicioDTO modelo);
        Task<bool> Eliminar(int id);
    }
}

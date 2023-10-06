using SistemaVenta.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DLL.Servicios.Contrato
{
    public interface IRolService
    {
        //Este metodo devuelve una lista que le voy a poner al metodo Lista()
        Task<List<RolDTO>> Lista();
    }
}

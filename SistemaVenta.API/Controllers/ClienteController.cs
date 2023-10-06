using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.API.Utilidad;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteServicio;

        public ClienteController(IClienteService clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<ClienteDTO>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _clienteServicio.Lista();
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpGet]
        [Route("ListaCliente")]

        public async Task<IActionResult> ListaCliente(int idUsuario)
        {
            var rsp = new Response<List<UsuarioDTO>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _clienteServicio.ListaCliente(idUsuario);
            }

            catch (Exception ex)
            {
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpPost]
        [Route("Guardar")]

        public async Task<IActionResult> Guardar([FromBody] ClienteDTO cliente)
        {
            var rsp = new Response<ClienteDTO>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _clienteServicio.Crear(cliente);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] ClienteDTO cliente)
        {
            var rsp = new Response<ClienteDTO>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _clienteServicio.Crear(cliente);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _clienteServicio.Eliminar(id);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
    }
}

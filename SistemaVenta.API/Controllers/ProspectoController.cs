using Microsoft.AspNetCore.Mvc;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.API.Utilidad;
using SistemaVenta.DTO;
using SistemaVenta.Models;
using SistemaVenta.DLL.Servicios;

namespace SistemaVenta.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ProspectoController : ControllerBase
    {
        private readonly IProspectoService _prospectoServicio;


        public ProspectoController(IProspectoService prospectoServicio)
        {
            _prospectoServicio = prospectoServicio;
        }
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Lista()
        {
            var rsp = new Response<List<ProspectoDTO>>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Lista();
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


        public async Task<IActionResult> Guardar([FromBody] ProspectoDTO prospecto)
        {
            var rsp = new Response<ProspectoDTO>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Crear(prospecto);
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
        public async Task<IActionResult> Editar([FromBody] ProspectoDTO prospecto)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.Status = true;
                rsp.Value = await _prospectoServicio.Editar(prospecto);
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
                rsp.Value = await _prospectoServicio.Eliminar(id);
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

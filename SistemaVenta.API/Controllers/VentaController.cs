using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaServicio;

        public VentaController(IVentaService ventaServicio)
        {
            _ventaServicio = ventaServicio;
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO cotizacion)
        {
            var rsp = new Response<VentaDTO>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _ventaServicio.Registrar(cotizacion);
            }

            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }

        [HttpGet]
        [Route("Historial")]
        //[Route("Historial/{buscarpor:string}/{numerocotizacion:string}/{fechaInicio:string}/{fechaFin:string}/")]

        public async Task<IActionResult> Historial(string? buscarpor, string? numerocotizacion, string? fechaInicio, string? fechaFin)
        {
            var rsp = new Response<List<VentaDTO>>();
            numerocotizacion = numerocotizacion is null ? "" : numerocotizacion;
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;
            buscarpor = buscarpor is null ? "" : buscarpor;
            try
            {
                rsp.Status = true;
                rsp.Value = await _ventaServicio.Historial(buscarpor, numerocotizacion, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                rsp.Status = false;
                rsp.Msg = ex.Message;
            }
            //TODAS LOS SOLICITUDES SERÁN RESPUESTAS EXITOSAS
            return Ok(rsp);
        }
        [HttpGet]
        [Route("Reporte")]

        public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
        {
            var rsp = new Response<List<ReporteDTO>>();
            try
            {
                rsp.Status = true;
                rsp.Value = await _ventaServicio.Reporte(fechaInicio, fechaFin);
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

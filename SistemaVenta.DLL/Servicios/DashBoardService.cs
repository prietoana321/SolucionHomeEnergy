using AutoMapper;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DLL.Servicios
{
    public class DashBoardService:IDashBoardService
    {
        private readonly IVentaRepository _ventaRepositorio;

        private readonly IGenericRepository<Servicio> _servicioRepositorio;

        private readonly IMapper _mapper;

        private readonly IGenericRepository<Estado> _estadoVentaRepositorio;


        public DashBoardService(IVentaRepository ventaRepositorio, IGenericRepository<Servicio> servicioRepositorio, IMapper mapper, IGenericRepository<Estado> estadoVentaRepositorio)
        {
            _ventaRepositorio = ventaRepositorio;
            _servicioRepositorio = servicioRepositorio;
            _mapper = mapper;
            _estadoVentaRepositorio = estadoVentaRepositorio;
        }
        //recibe la tabla de ventas, el siguiente
        private IQueryable<Venta> RetornarVentas(IQueryable<Venta> tablaVenta, int restarCantidadDias)
        {
            //el ? despues de datetime significa que permitira nullos
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
            //nos lo ordenará por fecha de registro
            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);
            //vamos a obtener la ultima fecha encontrada y a esa fecha le vamos a restar los dias
            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
        }
        //EL SIGUIENTE ES PARA MOSTRAR EN NUESTRO DASHBOARD EL NUMERO DE VENTAS, COMO UN DIGITO
        private async Task<int> TotalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            //validamos que si existan ventas

            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = RetornarVentas(_ventaQuery, -7);
                //vamos a obtener el total de ventas que han sido registradas estos 7 dias
                total = tablaVenta.Count();
            }
            return total;
        }

        //EL SIGUIENTE METODO MOSTRARÁ EL TOTAL DE INGRESOS DE LA ULTIMA SEMANA
        private async Task<string> TotalIngresosUltimaSemana()
        { //todo metodo retorna el tipo de aquí arriba
            decimal resultado = 0;

            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            //validamos que si existan ventas
            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = RetornarVentas(_ventaQuery, -7);
                //vamos a obtener el total de ventas que han sido registradas estos 7 dias
                resultado = tablaVenta.Select(v => v.Total).Sum(v => v.Value);
            }
            return Convert.ToString(resultado, new CultureInfo("es-CO"));
        }
        private async Task<int> TotalServicios()
        {

            IQueryable<Servicio> _productoQuery = await _servicioRepositorio.Consultar();
            int total = _productoQuery.Count();
            return total;
        }
        private async Task<Dictionary<string, int>> ventasUltimaSemana()
        {
            //variable diccionario oara ingresar es un string
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = RetornarVentas(_ventaQuery, -7);

                resultado = tablaVenta
                    .GroupBy(v => v.FechaRegistro.Value.Date)
                    .OrderBy(g => g.Key)
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);
            }
            return resultado;
        }
        public async Task<int> TotalCerradas(string Nombre)
        {
            

            IQueryable<Estado> _estadooQueryCerrado = await _estadoVentaRepositorio.Consultar();
            int total = _estadooQueryCerrado.Count();
         

            try
            {
                if (Nombre == "CERRADO")
                {
                    int totalCerradas = _estadooQueryCerrado.Count();
                    return total;
                }
                else {
                    return total;

                }
                
            }
            catch
            {
                throw;
            }
           
        }
    
        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO vmDashboard = new DashBoardDTO();
            try
            {
                vmDashboard.TotalVentas = await TotalVentasUltimaSemana();
                vmDashboard.TotalIngresos = await TotalIngresosUltimaSemana();
                vmDashboard.TotalServicios = await TotalServicios();
                List<VentasSemanaDTO> listaVentaSemana = new List<VentasSemanaDTO>();

                foreach (KeyValuePair<string, int> item in await ventasUltimaSemana())
                {
                    listaVentaSemana.Add(new VentasSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }
                vmDashboard.VentasUltimaSemana = listaVentaSemana;
            }
            catch
            {
                throw;
            }
            return vmDashboard;
        }
    }
}

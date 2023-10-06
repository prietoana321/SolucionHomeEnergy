using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DLL.Servicios
{
    public class EstadoService:IEstadoService
    {
        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepositorio;
        private readonly IGenericRepository<Estado> _estadoRepositorio;
        private readonly IMapper _mapper;

        public EstadoService(IVentaRepository ventaRepositorio, IGenericRepository<DetalleVenta> detalleVentaRepositorio, IGenericRepository<Estado> estadoRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _estadoRepositorio = estadoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<EstadoDTO>> Lista()
        {

            try
            {
                var queryEstado = await _estadoRepositorio.Consultar();
                var listaEstados = queryEstado.Include(u => u.DetalleVenta).ToList();
                return _mapper.Map<List<EstadoDTO>>(listaEstados);
            }
            catch
            {
                throw;
            }
        }
        public async Task<EstadoDTO> Crear(EstadoDTO modelo)
        {
            try
            {
                var estadoCreado = await _estadoRepositorio.Crear(_mapper.Map<Estado>(modelo));
                if (estadoCreado.IdEstado == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el Cliente");
                }
                return _mapper.Map<EstadoDTO>(estadoCreado);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(EstadoDTO modelo)
        {

            try
            {
                var estadoModelo = _mapper.Map<Estado>(modelo);
                var estadoCreado = await _estadoRepositorio.Crear(_mapper.Map<Estado>(modelo));
                var estadoEncontrado = await _estadoRepositorio.Obtener(u => u.IdEstado == estadoModelo.IdEstado);
                if (estadoEncontrado == null)
                {
                    throw new TaskCanceledException("Estado no existe");
                }

                estadoEncontrado.Nombre = estadoModelo.Nombre;


                bool respuesta = await _estadoRepositorio.Editar(estadoEncontrado);

                if (respuesta == false)
                {
                    throw new TaskCanceledException("No se pudo editar");
                }
                return respuesta;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var estadoEncontrado = await _estadoRepositorio.Obtener(u => u.IdEstado == id);
                //donde usuario encontrado en la segunda parte no lleva await 

                if (estadoEncontrado == null)
                {
                    throw new TaskCanceledException("El estado no existe");
                }

                bool respuesta = await _estadoRepositorio.Eliminar(estadoEncontrado);

                if (respuesta == false)
                {
                    throw new TaskCanceledException("No se pudo eliminar");

                }
                return respuesta;

            }
            catch
            {
                throw;
            }
        }

    }
}

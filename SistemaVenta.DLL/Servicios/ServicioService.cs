using AutoMapper;
using SistemaVenta.DAL.Repositorios.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.Models;
using Microsoft.EntityFrameworkCore;

namespace SistemaVenta.DLL.Servicios
{
    public class ServicioService:IServicioService
    {
        private readonly IGenericRepository<Servicio> _servicioRepositorio;
        private readonly IMapper _mapper;

        public ServicioService(IGenericRepository<Servicio> servicioRepositorio, IMapper mapper)
        {
            _servicioRepositorio = servicioRepositorio;
            _mapper = mapper;
        }
        public async Task<List<ServicioDTO>> Lista()
        {
            try
            {
                var queryProducto = await _servicioRepositorio.Consultar();
                var listaProductos = queryProducto.Include(cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<ServicioDTO>>(listaProductos).ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<ServicioDTO> Crear(ServicioDTO modelo)
        {
            try
            {
                var servicioCreado = await _servicioRepositorio.Crear(_mapper.Map<Servicio>(modelo));
                if (servicioCreado.IdServicio == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el Servicio");
                }
                return _mapper.Map<ServicioDTO>(servicioCreado);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ServicioDTO modelo)
        {
            try
            {
                var servicioModelo = _mapper.Map<Servicio>(modelo);
                var servicioEncontrado = await _servicioRepositorio.Obtener(u => u.IdServicio == servicioModelo.IdServicio);
                if (servicioEncontrado == null)
                {
                    throw new TaskCanceledException("No existe el Servicio");
                }
                servicioEncontrado.Nombre = servicioModelo.Nombre;
                servicioEncontrado.IdCategoria = servicioModelo.IdCategoria;
                servicioEncontrado.Precio = servicioModelo.Precio;
                servicioEncontrado.EsActivo = servicioModelo.EsActivo;

                bool respuesta = await _servicioRepositorio.Editar(servicioEncontrado);
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
                var servicioEncontrado = await _servicioRepositorio.Obtener(p => p.IdServicio == id);
                if (servicioEncontrado == null)
                {
                    throw new TaskCanceledException("El servicio no existe");
                }
                bool respuesta = await _servicioRepositorio.Eliminar(servicioEncontrado);
                if (respuesta == false)
                {
                    throw new TaskCanceledException("El servicio no se elimino con exito");
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

using AutoMapper;
using Microsoft.AspNetCore.Hosting;
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
    public class ImageService : IImageService
    {
        private readonly IGenericRepository<FileData> _fileDataRepositorio;

        private readonly IGenericRepository<Imagen> _imagenRepositorio;

        private readonly IMapper _mapper;

        private IWebHostEnvironment environment;

        public ImageService(IWebHostEnvironment env, IGenericRepository<FileData> fileRepositorio, IGenericRepository<Imagen> imagenRepositorio, IMapper mapper)
        {
            this.environment = env;
            _fileDataRepositorio = fileRepositorio;
            _imagenRepositorio = imagenRepositorio;
            _mapper = mapper;
        }
        public async Task<List<Imagen>> Lista()
        {
            try
            {

                var queryProducto = await _imagenRepositorio.Consultar();
                return queryProducto.ToList();
            }
            catch
            {
                throw;
            }
        }
        public async Task<bool> Add(ImagenDTO modelo)
        {
            try
            {
                var prospectoCreado = await _imagenRepositorio.Crear(_mapper.Map<Imagen>(modelo));
                if (prospectoCreado.IdImagen == 0)
                {
                    throw new TaskCanceledException("No se pudo crear la imagen");
                }
                return true;

            }
            catch
            {
                throw;
            }
        }

        public async Task<ImagenDTO> Crear(ImagenDTO modelo)
        {
            try
            {
                var prospectoCreado = await _imagenRepositorio.Crear(_mapper.Map<Imagen>(modelo));
                if (prospectoCreado.IdImagen == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el Prospecto");
                }
                return _mapper.Map<ImagenDTO>(prospectoCreado);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ImagenDTO modelo)
        {
            try
            {
                var prospectoModelo = _mapper.Map<Imagen>(modelo);
                var prospectoCreado = await _imagenRepositorio.Editar(_mapper.Map<Imagen>(modelo));
                var prospectoEncontrado = await _imagenRepositorio.Obtener(u => u.IdImagen == prospectoModelo.IdImagen);
                if (prospectoEncontrado == null)
                {
                    throw new TaskCanceledException("imagen no existe");
                }

                prospectoEncontrado.Nombre = prospectoModelo.Nombre;


                bool respuesta = await _imagenRepositorio.Editar(prospectoEncontrado);

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
                var prospectoEncontrado = await _imagenRepositorio.Obtener(u => u.IdImagen == id);
                //donde usuario encontrado en la segunda parte no lleva await 

                if (prospectoEncontrado == null)
                {
                    throw new TaskCanceledException("El prospecto no existe");
                }

                bool respuesta = await _imagenRepositorio.Eliminar(prospectoEncontrado);

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

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
    public class UsuarioService:IUsuarioService
    {

       
        private readonly IGenericRepository<ClienteUsuario> _clienteUsuarioRepositorio;
        private readonly IGenericRepository<Cliente> _clienteRepositorio;
        private readonly IGenericRepository<Prospecto> _prospectoRepositorio;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;

        public UsuarioService(IGenericRepository<ClienteUsuario> clienteUsuarioRepositorio, IGenericRepository<Cliente> clienteRepositorio, IGenericRepository<Prospecto> prospectoRepositorio, IMapper mapper, IGenericRepository<Usuario> usuarioRepositorio)
        {
            _clienteUsuarioRepositorio = clienteUsuarioRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _prospectoRepositorio = prospectoRepositorio;
            _mapper = mapper;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar();
                var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch
            {
                throw;
            }
        }
        public async Task<SesionDTO> ValidarCredenciales(string Correo, string Clave)
        {
            try
            {
                var queryUsuario = await _usuarioRepositorio.Consultar(u => u.Correo == Correo && u.Clave == Clave);
                if (queryUsuario.FirstOrDefault() == null)
                {
                    throw new TaskCanceledException("El usuario no existe");

                }
                Usuario devolverUsuario = queryUsuario.Include(rol => rol.IdRolNavigation).First();
                //este usuario de la linea de arriba o devolver usuario, es del tipo Usuario, asi que debemos pasarlo por mapper y convertirlo de tipo SesionDTO
                return _mapper.Map<SesionDTO>(devolverUsuario);


            }
            catch
            {
                throw;
            }
        }
        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                //nuestro usuariocreado recibe un usuario, pero no es del tipo dto, asi que para recibirlo en _usuarioRepositorio debemos covertirlo, así lo aceptará el modelo
                var UsuarioCreado = await _usuarioRepositorio.Crear(_mapper.Map<Usuario>(modelo));
                if (UsuarioCreado.IdUsuario == 0)
                {
                    throw new TaskCanceledException("Usuario no se pudo crear");
                }
                //con el await obtenemos, si es cero el usuario fallo, sino, continuaria aquí
                var query = await _usuarioRepositorio.Consultar(u => u.IdUsuario == UsuarioCreado.IdUsuario);
                UsuarioCreado = query.Include(rol => rol.IdRolNavigation).First();
                return _mapper.Map<UsuarioDTO>(UsuarioCreado);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);
                
                if(usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("Usuario no existe");
                }
                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;

                bool respuesta = await _usuarioRepositorio.Editar(usuarioEncontrado);

                if (!respuesta)
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
                var usuarioEncontrado = await _usuarioRepositorio.Obtener(u => u.IdUsuario == id);
                //donde usuario encontrado en la segunda parte no lleva await 

                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }

                bool respuesta = await _usuarioRepositorio.Eliminar(usuarioEncontrado);

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

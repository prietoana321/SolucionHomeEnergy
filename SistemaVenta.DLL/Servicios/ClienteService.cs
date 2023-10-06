using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.Repositorios.Contrato;
using SistemaVenta.DLL.Servicios.Contrato;
using SistemaVenta.DTO;
using SistemaVenta.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace SistemaVenta.DLL.Servicios
{
    public class ClienteService:IClienteService
    {
        private readonly IGenericRepository<ClienteUsuario> _clienteUsuarioRepositorio;
        private readonly IGenericRepository<Cliente> _clienteRepositorio;
        private readonly IGenericRepository<Usuario> _usuarioRepositorio;
        private readonly IGenericRepository<Prospecto> _prospectoRepositorio;
        private readonly IMapper _mapper;

        public ClienteService(IGenericRepository<ClienteUsuario> clienteUsuarioRepositorio, IGenericRepository<Cliente> clienteRepositorio, IGenericRepository<Usuario> usuarioRepositorio, IGenericRepository<Prospecto> prospectoRepositorio, IMapper mapper)
        {
            _clienteUsuarioRepositorio = clienteUsuarioRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _prospectoRepositorio = prospectoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<ClienteDTO>> Lista()
        {
           
            try
            {
                var queryCliente = await _clienteRepositorio.Consultar();
                var listaClientes = queryCliente;
                return _mapper.Map<List<ClienteDTO>>(listaClientes);
            }
            catch
            {
                throw;
            }
        }
        public async Task<ClienteDTO> Crear(ClienteDTO modelo)
        {
            try
            {
                var clienteCreado = await _clienteRepositorio.Crear(_mapper.Map<Cliente>(modelo));
                if (clienteCreado.IdCliente == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el Cliente");
                }
                return _mapper.Map<ClienteDTO>(clienteCreado);

            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(ClienteDTO modelo)
        {
            try
            {
                var clienteModelo = _mapper.Map<Cliente>(modelo);
                var clienteCreado = await _clienteRepositorio.Crear(_mapper.Map<Cliente>(modelo));
                var clienteEncontrado = await _clienteRepositorio.Obtener(u => u.IdCliente == clienteModelo.IdCliente);
                if (clienteEncontrado == null)
                {
                    throw new TaskCanceledException("Cliente no existe");
                }
                
                clienteEncontrado.Nombre = clienteModelo.Nombre;
                clienteEncontrado.Fachadaimg = clienteModelo.Fachadaimg;
                clienteEncontrado.Idauditor = clienteModelo.Idauditor;
                clienteEncontrado.Url = clienteModelo.Url;
                clienteEncontrado.Direccion = clienteModelo.Direccion;
                clienteEncontrado.Contacto = clienteModelo.Contacto;
                clienteEncontrado.RazonSocial = clienteModelo.RazonSocial;
                clienteEncontrado.EsActivo = clienteModelo.EsActivo;
                clienteEncontrado.Fecha = clienteModelo.Fecha;


                bool respuesta = await _clienteRepositorio.Editar(clienteEncontrado);

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
                var clienteEncontrado = await _clienteRepositorio.Obtener(u => u.IdCliente == id);
                //donde usuario encontrado en la segunda parte no lleva await 

                if (clienteEncontrado == null)
                {
                    throw new TaskCanceledException("El cliente no existe");
                }

                bool respuesta = await _clienteRepositorio.Eliminar(clienteEncontrado);

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
        public async Task<List<UsuarioDTO>> ListaCliente(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = await _usuarioRepositorio.Consultar();
            IQueryable<ClienteUsuario> tbClienteUsuario = await _clienteUsuarioRepositorio.Consultar();
            IQueryable<Cliente> tbCliente = await _clienteRepositorio.Consultar(u => u.Idauditor == idUsuario);
            IQueryable<Prospecto> tbProspecto = await _prospectoRepositorio.Consultar(mr => mr.Idauditor == idUsuario);

            try
            {
                IQueryable<Usuario> tbResultado = (from u in tbCliente
                                                   join mr in tbClienteUsuario on u.IdCliente equals mr.IdCliente
                                                   join m in tbUsuario on u.Idauditor equals m.IdUsuario
                                                   select m).AsQueryable();

                var listaClientes = tbResultado.ToList();
                return _mapper.Map<List<UsuarioDTO>>(listaClientes);
            }
            catch
            {
                throw;
            }
        }
    }
}

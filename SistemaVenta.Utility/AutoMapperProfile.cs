using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Models;

namespace SistemaVenta.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol, RolDTO>().ReverseMap();
            #endregion Rol


            #region Menu
            CreateMap<Menu, MenuDTO>().ReverseMap();
            #endregion Menu


            #region Usuario
            CreateMap<Usuario, UsuarioDTO>().ForMember(destino => destino.RolDescripcion, opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre))
            .ForMember(destino =>
            destino.EsActivo,
            opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0));

            CreateMap<Usuario, SesionDTO>()
                .ForMember(destino =>
                destino.RolDescripcion,
                opt => opt.MapFrom(origen => origen.IdRolNavigation.Nombre));

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino => destino.IdRolNavigation, opt => opt.Ignore()
                )
                .ForMember(destino =>
            destino.EsActivo,
            opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false));
            #endregion Usuario

            #region Categoria
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
            #endregion Categoria

            #region Servicio
            CreateMap<Servicio, ServicioDTO>()
                .ForMember(destino => destino.DescripcionCategoria,
                opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Nombre)
                )
                .ForMember(destino => destino.Precio,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio.Value, new CultureInfo("es-CO")))
                )
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0));

            CreateMap<ServicioDTO, Servicio>()
                .ForMember(destino => destino.IdCategoriaNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino => destino.Precio,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-CO")))
                )
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false));
            #endregion Servicio

            #region Venta
            CreateMap<Venta, VentaDTO>()
                .ForMember(destino =>
                destino.TotalTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-CO")))
                )
                .ForMember(destino =>
                destino.FechaRegistro,
                opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                );
            CreateMap<VentaDTO, Venta>()
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.TotalTexto, new CultureInfo("es-CO")))
                );

            #endregion

            #region DetalleVenta
            CreateMap<DetalleVenta, DetalleVentaDTO>()
                .ForMember(destino => destino.DescripcionServicio,
                opt => opt.MapFrom(origen => origen.IdServicioNavigation.Nombre)
                )
                .ForMember(destino =>
                destino.PrecioTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-CO"))
                ))
                .ForMember(destino =>
                destino.TotalTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-CO")))
                );
            CreateMap<DetalleVentaDTO, DetalleVenta>()

                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen => Convert.ToString(origen.PrecioTexto, new CultureInfo("es-CO"))
                ))
                .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen => Convert.ToString(origen.TotalTexto, new CultureInfo("es-CO")))
                );
            CreateMap<Estado, DetalleVentaDTO>()
                .ForMember(destino =>
                destino.EstadoDescripcion,
                opt => opt.MapFrom(origen => origen.Nombre));

            #endregion

            #region Prospecto
            CreateMap<Prospecto, ProspectoDTO>()
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0));

            CreateMap<ProspectoDTO, Prospecto>()
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false));

            #endregion Prospecto
            #region Cliente
            CreateMap<Cliente, ClienteDTO>()
                .ForMember(destino => destino.DescripcionProspecto,
                opt => opt.MapFrom(origen => origen.IdProspectoNavigation.Nombre)
                )
                .ForMember(destino => destino.Detalle,
                opt => opt.MapFrom(origen => origen.IdProspectoNavigation.Detalle)
                )
                .ForMember(destino => destino.DescripcionAuditor,
                opt => opt.MapFrom(origen => origen.Idauditor)
                )
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == true ? 1 : 0));

            CreateMap<ClienteDTO, Cliente>()
                .ForMember(destino => destino.IdProspectoNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino => destino.Idauditor,
                opt => opt.MapFrom(origen => origen.Idauditor)
                )
                .ForMember(destino => destino.Detalle,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.EsActivo,
                opt => opt.MapFrom(origen => origen.EsActivo == 1 ? true : false));
           
            CreateMap<Prospecto, ClienteDTO>()
               .ForMember(destino =>
               destino.DescripcionProspecto,
               opt => opt.MapFrom(origen => origen.IdProspecto));
            #endregion Cliente

            #region Estado
            CreateMap<Estado, EstadoDTO>().ReverseMap();
            #endregion Estado
        }

    }
}

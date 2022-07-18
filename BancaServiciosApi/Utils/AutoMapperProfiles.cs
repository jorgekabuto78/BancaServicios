using AutoMapper;
using BancaServiciosApi.Dtos;
using BancaServiciosApi.Models;

namespace BancaServiciosApi.Utils
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CuentaDTO, Cuenta>();
            CreateMap<MovimientoDTO, Movimiento>();
        }
    }
}

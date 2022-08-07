using AutoMapper;
using BancaServiciosApi.Dtos;
using BancaServiciosApi.Models;
using BancaServiciosApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BancaServiciosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public MovimientosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int movimientoId)
        {
            var movimiento = await this.context.Movimientos.Include(m => m.Cuenta).Where(m => m.MovimientoId == movimientoId).FirstOrDefaultAsync();

            if (movimiento == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(movimiento);
            }
        }

        [HttpGet("ObtenerMovimientos")]
        public async Task<ActionResult> ObtenerMovimientos()
        {
            var movimientos = await this.context.Movimientos.ToListAsync();

            if (movimientos == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(movimientos);
            }
        }

        [HttpPost("Registrar/{cuentaId:int}")]
        public async Task<ActionResult> Post([FromRoute]int cuentaId, [FromBody] MovimientoDTO movimientoDTO)
        {
            var cuenta = await this.context.Cuentas.Include(c => c.Cliente).Where(c => c.CuentaId == cuentaId).FirstOrDefaultAsync();

            if (cuenta == null)
            {
                return NotFound($"El id de la cuenta '{cuentaId}' no existe");
            }

            var movimiento = mapper.Map<Movimiento>(movimientoDTO);

            if (movimiento.TipoMovimiento.ToLower().Equals("deposito"))
            {
                movimiento.Saldo = cuenta.SaldoInicial + movimiento.Valor;           
                cuenta.SaldoInicial = movimiento.Saldo;
                movimiento.Cuenta = cuenta;
            }

            if (movimiento.TipoMovimiento.ToLower().Equals("retiro"))
            {
                if (cuenta.SaldoInicial == 0)
                {
                    return BadRequest("El saldo de la cuenta es 0");
                }
                movimiento.Saldo = cuenta.SaldoInicial - movimiento.Valor;
                cuenta.SaldoInicial = movimiento.Saldo;
                movimiento.Cuenta = cuenta;
            }

            this.context.Movimientos.Add(movimiento);
            this.context.Entry(cuenta).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
            return Ok(movimiento);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Movimiento movimiento)
        {
            this.context.Entry(movimiento).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
            return Ok(movimiento);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int movimientoId)
        {
            var existeMovimiento = await this.context.Movimientos.AnyAsync(m => m.MovimientoId == movimientoId);

            if (existeMovimiento)
            {
                var movimiento = await this.context.Movimientos.Where(m => m.MovimientoId == movimientoId).FirstOrDefaultAsync();
                this.context.Movimientos.Remove(movimiento);
                await this.context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


    }
}

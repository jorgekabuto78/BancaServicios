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
    public class CuentasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;    

        public CuentasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int cuentaId)
        {
            var cuenta = this.context.Cuentas.Include(c => c.Cliente).Where(c => c.CuentaId == cuentaId).FirstOrDefault();

            if (cuenta == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cuenta);
            }
        }

        [HttpGet("ObtenerCuentasActivas")]
        public async Task<ActionResult> ObtenerCuentasActivas()
        {
            var cuentas = this.context.Cuentas.Where(c => c.Estado.Equals(true)).FirstOrDefault();

            if (cuentas == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(cuentas);
            }
        }

        [HttpPost("{clienteId:int}")]
        public async Task<ActionResult<CuentaDTO>> Post([FromRoute]int clienteId, [FromBody] CuentaDTO cuentaDTO)
        {
            bool existeCliente = this.context.Clientes.Any(c => c.ClienteId == clienteId);

            if (!existeCliente)
            {
                return NotFound($"El id de cliente '{clienteId}' no existe");
            }
                        
            var cuenta = mapper.Map<Cuenta>(cuentaDTO);

            var cliente = await this.context.Clientes.FirstOrDefaultAsync(c => c.ClienteId == clienteId);

            cuenta.Cliente = cliente;
            cuenta.Estado = true;

            this.context.Cuentas.Add(cuenta);

            await this.context.SaveChangesAsync();

            return Ok(cuentaDTO);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Cuenta cuenta)
        {
            if (cuenta.CuentaId == 0)
            {
                return BadRequest("Se debe especificar el id de la cuenta");
            }

            this.context.Entry(cuenta).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch]
        // No debo permitir modificar el cliente de la cuenta - implementar
        public async Task<ActionResult> Patch(Cuenta cuenta)
        {
            if (cuenta.CuentaId == 0)
            {
                return BadRequest("Se debe especificar el id de la cuenta");
            }

            this.context.Entry(cuenta).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int cuentaId)
        {
            var existeCuenta = this.context.Cuentas.Any(c => c.CuentaId == cuentaId);

            if (existeCuenta)
            {
                var cuenta = this.context.Cuentas.Where(c => c.CuentaId == cuentaId).FirstOrDefault();
                this.context.Cuentas.Remove(cuenta);
                await this.context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

    }
}

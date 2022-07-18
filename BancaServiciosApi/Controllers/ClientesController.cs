using BancaServiciosApi.Models;
using BancaServiciosApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BancaServiciosApi.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ClientesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int clienteId)
        {
            var cliente = this.context.Clientes.Where(c => c.ClienteId == clienteId).FirstOrDefault();

            if (cliente != null)
            {
                return Ok(cliente);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("ObtenerClientesActivos")]
        public async Task<ActionResult<List<Cliente>>> ObtenerClientesActivos()
        {
            var clientes = this.context.Clientes.Where(c => c.Estado.Equals(true)).ToList();

            if (clientes != null)
            {
                return Ok(clientes);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(Cliente cliente)
        {
            this.context.Add(cliente);
            await this.context.SaveChangesAsync();
            return Ok(cliente);
        }

        [HttpPut]
        public async Task<ActionResult> Put(Cliente cliente)
        {
            if (cliente.ClienteId == 0)
            {
                return BadRequest("Se debe especificar el id del cliente");
            }

            this.context.Entry(cliente).State = EntityState.Modified;
            await this.context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int clienteId)
        {
            var existeCliente = this.context.Clientes.Any(c => c.ClienteId == clienteId);

            if (existeCliente)
            {
                var cliente = this.context.Clientes.Where(c => c.ClienteId == clienteId).FirstOrDefault();
                this.context.Clientes.Remove(cliente);
                await this.context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{clienteId:int}/ObtenerEstadoCuenta/{fecini}/{fecfin}")]
        public async Task<ActionResult> ObtenerEstadoCuenta(int clienteId, string fecini, string fecfin)
        {
            var inicio = DateTime.ParseExact(fecini, "yyyy-MM-dd", null);
            var fin = DateTime.ParseExact(fecfin, "yyyy-MM-dd", null);

            var movimientos = await this.context.Movimientos.Include(m => m.Cuenta).Include(m => m.Cuenta.Cliente)
                              .Where(m => m.Cuenta.Cliente.ClienteId.Equals(clienteId) && (m.FechaMovimiento >= inicio && m.FechaMovimiento <= fin))
                              .ToListAsync();
            
            if (movimientos.Count > 0)
            {
                return Ok(movimientos);
            }
            else
            {
                return Content("El cliente no registra movimientos");
            }

        }

    }
}

using API.Ecommerce.DTOs.Pagos;
using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using API.Ecommerce.Services.Interfaces;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _repo;
        private readonly ApplicationDbContext _ctx;
        private readonly ILogger<PagoService> _logger;

        public PagoService(IPagoRepository repo, ApplicationDbContext ctx, ILogger<PagoService> logger)
        {
            _repo = repo;
            _ctx = ctx;
            _logger = logger;
        }

        private PagoDto Map(Pago p) => new PagoDto
        {
            Id = p.Id,
            PedidoId = p.PedidoId,
            ClienteId = p.ClienteId,
            Monto = p.Monto,
            Metodo = p.Metodo,
            Referencia = p.Referencia,
            Fecha = p.Fecha,
            Detalles = p.Detalles?.Select(d => new PagoDetalleDto
            {
                Id = d.Id,
                Monto = d.Monto,
                Metodo = d.Metodo,
                Referencia = d.Referencia
            }).ToList() ?? new List<PagoDetalleDto>()
        };

        public async Task<(IEnumerable<PagoDto> Items, int Total)> GetPagedAsync(int page, int pageSize, int? clienteId, int? pedidoId, DateTime? desde, DateTime? hasta)
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, clienteId, pedidoId, desde, hasta);
            return (items.Select(Map), total);
        }

        public async Task<PagoDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            return p == null ? null : Map(p);
        }

        public async Task<PagoDto> CreateAsync(PagoCreateDto dto)
        {
            // Validaciones simples
            if (dto.PedidoId.HasValue)
            {
                var pedido = await _ctx.Pedidos.FindAsync(dto.PedidoId.Value);
                if (pedido == null) throw new InvalidOperationException("Pedido no encontrado.");
            }
            if (dto.ClienteId.HasValue)
            {
                var cliente = await _ctx.Clientes.FindAsync(dto.ClienteId.Value);
                if (cliente == null) throw new InvalidOperationException("Cliente no encontrado.");
            }

            var pago = new Pago
            {
                PedidoId = dto.PedidoId,
                ClienteId = dto.ClienteId,
                Monto = dto.Monto,
                Metodo = dto.Metodo,
                Referencia = dto.Referencia,
                Fecha = DateTime.UtcNow
            };

            if (dto.Detalles != null && dto.Detalles.Any())
            {
                pago.Detalles = dto.Detalles.Select(d => new PagoDetalle
                {
                    Monto = d.Monto,
                    Metodo = d.Metodo,
                    Referencia = d.Referencia
                }).ToList();
            }

            var creado = await _repo.AddAsync(pago);

            // Opcional: actualizar estado del pedido (por ejemplo, marcar pagado)
            // if (creado.PedidoId.HasValue) { ... }

            return Map(creado);
        }

        public async Task<PagoDto?> UpdateAsync(PagoUpdateDto dto)
        {
            var p = await _repo.GetByIdAsync(dto.Id);
            if (p == null) return null;

            p.Monto = dto.Monto;
            p.Metodo = dto.Metodo;
            p.Referencia = dto.Referencia;

            // No manejamos editar detalles complejos aquí (podemos añadir endpoints para eso)
            var actualizado = await _repo.UpdateAsync(p);
            return Map(actualizado);
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<IEnumerable<PagoDto>> GetByPedidoAsync(int pedidoId)
        {
            var pagos = await _repo.GetByPedidoAsync(pedidoId);
            return pagos.Select(Map);
        }

        public async Task<IEnumerable<PagoDto>> GetByClienteAsync(int clienteId)
        {
            var pagos = await _repo.GetByClienteAsync(clienteId);
            return pagos.Select(Map);
        }
    }
}

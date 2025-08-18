using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CateonCook.Modelos;

namespace CateonCook.Repositorios;

public enum PeriodoResumen { Diario, Mensual, Anual }

public sealed record VentaResumen(DateTime Periodo, int Unidades, decimal Subtotal, decimal Iva, decimal Total);

public interface IVentaRepository
{
    Task InitAsync();

    // CRUD básico
    Task InsertAsync(Venta venta, CancellationToken ct = default);
    Task UpdateAsync(Venta venta, CancellationToken ct = default);
    Task DeleteAsync(string id, CancellationToken ct = default);
    Task<Venta?> GetByIdAsync(string id, CancellationToken ct = default);

    // Consultas
    Task<IReadOnlyList<Venta>> GetByDateRangeAsync(DateTime desde, DateTime hasta, CancellationToken ct = default);

    // Resumen agregado por periodo (día/mes/año)
    Task<IReadOnlyList<VentaResumen>> GetResumenAsync(DateTime desde, DateTime hasta, PeriodoResumen periodo, CancellationToken ct = default);
}


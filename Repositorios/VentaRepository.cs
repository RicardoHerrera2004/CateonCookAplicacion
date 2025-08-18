using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using CateonCook.Modelos;
using CateonCook.Repositorios;

namespace CateonCook.Repositorios;

public sealed class VentaRepository : IVentaRepository
{
    private readonly ISQLiteConnection _provider;
    private SQLiteAsyncConnection Conn => _provider.GetConnection();

    public VentaRepository(ISQLiteConnection provider) => _provider = provider;

    public async Task InitAsync()
    {
        await Conn.CreateTableAsync<Venta>();
    }

    public async Task InsertAsync(Venta venta, CancellationToken ct = default)
    {
        await InitAsync();
        if (string.IsNullOrWhiteSpace(venta.Id)) venta.Id = Guid.NewGuid().ToString();
        await Conn.InsertAsync(venta);
    }

    public async Task UpdateAsync(Venta venta, CancellationToken ct = default)
    {
        await InitAsync();
        await Conn.UpdateAsync(venta);
    }

    public async Task DeleteAsync(string id, CancellationToken ct = default)
    {
        await InitAsync();
        var v = await GetByIdAsync(id, ct);
        if (v is not null) await Conn.DeleteAsync(v);
    }

    public async Task<Venta?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        await InitAsync();
        return await Conn.Table<Venta>().Where(v => v.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<Venta>> GetByDateRangeAsync(DateTime desde, DateTime hasta, CancellationToken ct = default)
    {
        await InitAsync();
        // Normalizamos límites (incluye todo el día final)
        var d0 = desde.Date;
        var d1 = hasta.Date.AddDays(1).AddTicks(-1);
        return await Conn.Table<Venta>()
                         .Where(v => v.FechaVenta >= d0 && v.FechaVenta <= d1)
                         .OrderBy(v => v.FechaVenta)
                         .ToListAsync();
    }

    public async Task<IReadOnlyList<VentaResumen>> GetResumenAsync(DateTime desde, DateTime hasta, PeriodoResumen periodo, CancellationToken ct = default)
    {
        await InitAsync();
        // Usamos strftime para agrupar en SQLite.
        string keyFmt = periodo switch
        {
            PeriodoResumen.Diario => "%Y-%m-%d",
            PeriodoResumen.Mensual => "%Y-%m-01",
            PeriodoResumen.Anual => "%Y-01-01",
            _ => "%Y-%m-%d"
        };

        // NOTA: en SQLite DateTime de .NET se guarda como TEXT (ISO8601) por defecto en sqlite-net-pcl.
        // strftime opera sobre ese texto.
        const string sql = @"SELECT 
                strftime($fmt, Fecha) AS PeriodoKey,
                SUM(Cantidad)         AS Unidades,
                SUM(Total) - SUM(Iva) AS Subtotal,
                SUM(Iva)              AS Iva,
                SUM(Total)            AS Total
            FROM Ventas
            WHERE Fecha >= $desde AND Fecha <= $hasta
            GROUP BY PeriodoKey
            ORDER BY PeriodoKey";

        var rows = await Conn.QueryAsync<_Row>(sql, new { fmt = keyFmt, desde, hasta });

        // Mapear PeriodoKey a DateTime (inicio del periodo)
        var list = rows.Select(r => new VentaResumen(
            Periodo: DateTime.Parse(r.PeriodoKey + " 00:00:00"),
            Unidades: r.Unidades,
            Subtotal: r.Subtotal,
            Iva: r.Iva,
            Total: r.Total
        )).ToList();

        return list;
    }

    private sealed class _Row
    {
        public string PeriodoKey { get; set; } = string.Empty;
        public int Unidades { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using SQLite;

namespace CateonCook.Repositorios;

public class LogService : ILogService
{
    private readonly SQLiteAsyncConnection _db;
    private readonly string _logFile;

    public LogService(IAppDatabase appDb)
    {
        _db = appDb.Connection;
        _logFile = Path.Combine(FileSystem.AppDataDirectory, $"logs_{DateTime.UtcNow:yyyyMMdd}.txt");
    }

    public async Task<List<ModelsInfra.LogEntry>> GetLogsAsync(
        DateTime? desde = null, DateTime? hasta = null, string? nivel = null)
    {
        var q = _db.Table<ModelsInfra.LogEntry>(); 

        if (desde.HasValue)
            q = q.Where(x => x.FechaUtc >= desde.Value);

        if (hasta.HasValue)
            q = q.Where(x => x.FechaUtc < hasta.Value);

        if (!string.IsNullOrWhiteSpace(nivel))
            q = q.Where(x => x.Nivel == nivel);

        var lista = await q.ToListAsync();
        return lista.OrderByDescending(x => x.FechaUtc).ToList();
    }


    public Task LogInfoAsync(string origen, string mensaje, string? detalle = null)
        => WriteAsync("INFO", origen, mensaje, detalle);

    public Task LogWarnAsync(string origen, string mensaje, string? detalle = null)
        => WriteAsync("WARN", origen, mensaje, detalle);

    public Task LogErrorAsync(string origen, string mensaje, Exception? ex = null)
    {
        var detalle = ex is null ? null : $"{ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}";
        return WriteAsync("ERROR", origen, mensaje, detalle);
    }

    private async Task WriteAsync(string nivel, string origen, string mensaje, string? detalle)
    {
        var entry = new ModelsInfra.LogEntry
        {
            Nivel = nivel,
            Origen = origen,
            Mensaje = mensaje,
            Detalle = detalle,
            FechaUtc = DateTime.UtcNow
        };

        await _db.InsertAsync(entry);

        // También a archivo (sencillo; rotación diaria por nombre)
        var line = $"{entry.FechaUtc:O}\t{nivel}\t{origen}\t{mensaje}{(string.IsNullOrEmpty(detalle) ? "" : "\t" + detalle)}";
        await File.AppendAllTextAsync(_logFile, line + Environment.NewLine, Encoding.UTF8);
    }
}

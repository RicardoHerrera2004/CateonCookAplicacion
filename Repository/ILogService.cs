using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateonCook.Repositorios;

public interface ILogService
{
    Task LogInfoAsync(string origen, string mensaje, string? detalle = null);
    Task LogWarnAsync(string origen, string mensaje, string? detalle = null);
    Task LogErrorAsync(string origen, string mensaje, Exception? ex = null);

    Task<List<ModelsInfra.LogEntry>> GetLogsAsync(DateTime? desde = null, DateTime? hasta = null, string? nivel = null);
}

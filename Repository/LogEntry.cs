using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CateonCook.Repositorios.ModelsInfra;

[Table("LogEntry")]
public class LogEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public DateTime FechaUtc { get; set; } = DateTime.UtcNow;

    [Indexed]
    public string Nivel { get; set; } = "INFO"; // INFO, WARN, ERROR

    public string Origen { get; set; } = "";    // clase/método
    public string Mensaje { get; set; } = "";
    public string? Detalle { get; set; }        // stacktrace u objeto JSON
}

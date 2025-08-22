using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CateonCook.Modelos
{
    [Table("Financiamiento")]
    public class Financiamiento
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Indexed]
        public string ProductoId { get; set; } = string.Empty;

        [Indexed]
        public string PlanPagoId { get; set; } = string.Empty;

        public decimal PrecioBase { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Iva { get; set; }

        public decimal Total { get; set; }

        public decimal ValorCuota { get; set; }

        public int NumeroCuotas { get; set; }

        public decimal TasaInteres { get; set; }

        public DateTime FechaCalculo { get; set; } = DateTime.UtcNow;
    }
}

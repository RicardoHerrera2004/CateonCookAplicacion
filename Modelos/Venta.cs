using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CateonCook.Modelos
{
    [Table("Ventas")]
    public class Venta
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Indexed]
        public DateTime FechaVenta { get; set; } = DateTime.UtcNow;

        [Indexed]
        public string ProductoId { get; set; } = string.Empty;

        //Nulo si fue a contado; relleno si se usó un plan de pago
        public string? PlanPagoId { get; set; } = string.Empty;

        public int Cantidad { get; set; } = 1;

        public decimal PrecioUnitario { get; set; } = 0m;

        public bool AfectoIva { get; set; } = true;

        public decimal Iva { get; set; } = 0m;

        public decimal Total { get; set; } = 0m;

        [Indexed]
        public MetodoPago MetodoPago { get; set; } = MetodoPago.TarjetaCredito;

    }
}

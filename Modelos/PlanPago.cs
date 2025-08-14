using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CateonCook.Modelos
{
    public enum MetodoPago
    {
        TarjetaCredito,
        EfectivoMasTarjeta,
        CreditoDirecto,
        Cheque
    }

    [Table("PlanesPago")]
    public class PlanPago
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [NotNull, MaxLength(120)]
        public string Nombre { get; set; } = string.Empty;

        public MetodoPago Metodo { get; set; } 

        public int NumeroCuotas { get; set; } = 0;

        public decimal TasaInteresAnual { get; set; } = 0m;

        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;
    }
}

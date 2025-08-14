using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CateonCook.Modelos
{
    //Materiales fijos de ollas
    public enum MaterialOlla
    {
        AceroInoxidable,
        HierroFundido,
        Aluminio,
        Cerámica,
        Vidrio,
        Otro
    }
    [Table("Productos")]
    public class Producto
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Indexed, MaxLength(64)]
        public string Sku { get; set; } = string.Empty;

        [Indexed, NotNull, MaxLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [Indexed,MaxLength(80)]
        public string Categoria { get; set; } = "Cocción";

        public MaterialOlla Material { get; set; } = MaterialOlla.AceroInoxidable;
        
        public double CapacidadLitros { get; set; } = 0;

        public decimal PrecioUnitario { get; set; } = 0m;
        
        public bool AfectoIva { get; set; } = true;

        public int Stock { get; set; } = 0;
        
        public int MinimoStock { get; set; } = 0;

        public string? Imagen { get; set; }
        
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
    }
}

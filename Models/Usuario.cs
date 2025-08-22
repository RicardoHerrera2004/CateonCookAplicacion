using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace CateonCook.Modelos
{
    public enum RolUsuario
    {
        Vendedor,
        Supervisor,
        Admin
    }

    [Table("Usuarios")]
    public class Usuario
    {
        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [MaxLength(120), NotNull]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(120), Indexed(Unique = true)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        public RolUsuario Rol { get; set; } = RolUsuario.Vendedor;

        public bool Activo { get; set; } = true;

        public DateTimeOffset? UltimoAcceso { get; set; }

        [MaxLength(100)]
        public string? Sucursal { get; set; }

        [MaxLength(20)]
        public string? CodigoEstablecimiento { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    }
}

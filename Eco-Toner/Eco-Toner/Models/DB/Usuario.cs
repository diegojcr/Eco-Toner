using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Correo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Usuario1 { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<OrdenDetalle> OrdenDetalleUsuarioAsignadoNavigations { get; set; } = new List<OrdenDetalle>();

    public virtual ICollection<OrdenDetalle> OrdenDetalleUsuarioPrevioNavigations { get; set; } = new List<OrdenDetalle>();

    public virtual ICollection<OrdenMaestro> OrdenMaestros { get; set; } = new List<OrdenMaestro>();
}

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

    public virtual ICollection<OrdenMaestro> OrdenMaestros { get; set; } = new List<OrdenMaestro>();
}

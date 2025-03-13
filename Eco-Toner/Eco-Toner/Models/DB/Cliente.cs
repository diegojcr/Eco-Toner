using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public int IdOficina { get; set; }

    public virtual ICollection<Contacto> Contactos { get; set; } = new List<Contacto>();

    public virtual Oficina IdOficinaNavigation { get; set; } = null!;

    public virtual ICollection<OrdenMaestro> OrdenMaestros { get; set; } = new List<OrdenMaestro>();
}

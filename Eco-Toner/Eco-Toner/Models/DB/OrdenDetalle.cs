using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class OrdenDetalle
{
    public int IdDetalle { get; set; }

    public string Descripción { get; set; } = null!;

    public int IdOrden { get; set; }

    public int IdUsuario { get; set; }

    public virtual OrdenMaestro IdOrdenNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}

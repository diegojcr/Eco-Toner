using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class OrdenMaestro
{
    public int IdOrden { get; set; }

    public string NoOrden { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public int IdCliente { get; set; }

    public int IdImpresora { get; set; }

    public int IdEstado { get; set; }

    public int IdUsuario { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Impresora IdImpresoraNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class OrdenMaestro
{
    public int IdOrden { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int IdCliente { get; set; }

    public int IdImpresora { get; set; }

    public int IdEstado { get; set; }

    public int UsuarioCreación { get; set; }

    public string? NumeroOrden { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Impresora IdImpresoraNavigation { get; set; } = null!;

    public virtual ICollection<OrdenDetalle> OrdenDetalles { get; set; } = new List<OrdenDetalle>();

    public virtual Usuario UsuarioCreaciónNavigation { get; set; } = null!;
}

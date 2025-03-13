using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class Impresora
{
    public int IdImpresora { get; set; }

    public int NumeroSerie { get; set; }

    public int IdModelo { get; set; }

    public virtual Modelo IdModeloNavigation { get; set; } = null!;

    public virtual ICollection<OrdenMaestro> OrdenMaestros { get; set; } = new List<OrdenMaestro>();
}

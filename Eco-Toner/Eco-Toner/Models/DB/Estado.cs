using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string Estado1 { get; set; } = null!;

    public virtual ICollection<OrdenMaestro> OrdenMaestros { get; set; } = new List<OrdenMaestro>();
}

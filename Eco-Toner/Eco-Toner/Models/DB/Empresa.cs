using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class Empresa
{
    public int IdEmpresa { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Oficina> Oficinas { get; set; } = new List<Oficina>();
}

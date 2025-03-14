using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class Oficina
{
    public int IdOficina { get; set; }

    public int IdEmpresa { get; set; }

    public string? Dirección { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

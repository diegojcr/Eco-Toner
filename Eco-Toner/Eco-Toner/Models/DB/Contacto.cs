using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class Contacto
{
    public int IdContacto { get; set; }

    public int Numero { get; set; }

    public int IdCliente { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;
}

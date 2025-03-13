using System;
using System.Collections.Generic;

namespace Eco_Toner.Models.DB;

public partial class Modelo
{
    public int IdModelo { get; set; }

    public int IdMarca { get; set; }

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual ICollection<Impresora> Impresoras { get; set; } = new List<Impresora>();
}

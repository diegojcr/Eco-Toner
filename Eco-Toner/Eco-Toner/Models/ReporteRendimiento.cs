namespace Eco_Toner.Models
{


    public class ReporteRendimiento
    {
        public int TrabajosCompletos { get; set; }
        public int TiempoDeTrabajo { get; set; }
        public int TiempoDeAsignacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public string Rol { get; set; }
    }

    public class FiltroRendimiento
    {
        public string Usuario { get; set; }
        public DateTime? FechaInicio { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime? FechaFin { get; set; } = DateTime.Now;

    }

}

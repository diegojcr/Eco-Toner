namespace Eco_Toner.Models
{
    public class ReporteTicketsAtendidos
    {
        public string Numero_Serie {  get; set; }
        public string Nombre_Modelo { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string Empresa {  get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public string Numero_Orden {  get; set; }
    }

    public class FiltroFechasTicketsAtendidos
    {
        public DateTime? FechaInicio { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime? FechaFin { get; set; } = DateTime.Now;
    }

    public class ReporteTicketsAtendidosViewModel
    {
        public List<ReporteTicketsAtendidos> Reportes { get; set; } = new List<ReporteTicketsAtendidos>();
    }
}

namespace Eco_Toner.Models
{
    public class ReporteImpresora
    {
        public string NombreModelo { get; set; }
        public string Marca {  get; set; }
        public int VecesArruinadas { get; set; }
    }
    public class FiltroFechas
    {
        public DateTime FechaInicio { get; set; } = DateTime.Now.AddMonths(-1);
        public DateTime FechaFin { get; set; } = DateTime.Now;
    }
}

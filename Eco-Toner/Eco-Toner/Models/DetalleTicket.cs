namespace Eco_Toner.Models
{
    public class DetalleTicket
    {
        public string Numero_Orden { get; set; }
        public string Descripción { get; set; }
        public string NOMBREPREVIO { get; set; }
        public string APELLIDOPREVIO { get; set; }
        public string NOMBREASIGNADO { get; set; }
        public string APELLIDOASIGNADO { get; set; }
        public DateTime? Fecha_Asignación { get; set; }
        public DateTime? Fecha_Inicio { get; set; }
        public DateTime? Fecha_Fin { get; set; }
    }
}

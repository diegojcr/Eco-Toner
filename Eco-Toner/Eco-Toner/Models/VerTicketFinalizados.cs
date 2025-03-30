namespace Eco_Toner.Models
{
    public class VerTicketFinalizados
    {
        public string Numero_Serie { get; set; }
        public string Nombre_Modelo { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string Empresa { get; set; }
        public string Dirección { get; set; }
        public string Descripción { get; set; }
        public string NombreTecnico { get; set; }
        public string ApellidoTecnico { get; set; }

        public string numero_orden { get; set; }

        public DateTime Fecha_Asignación { get; set; }

        public DateTime Fecha_Fin {  get; set; }


    }
}

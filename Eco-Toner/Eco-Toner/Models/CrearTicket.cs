namespace Eco_Toner.Models
{
    public class CrearTicket
    {
        public string Correo_Cliente { get; set; }
        public string Serie_Impresora { get; set; }
        public string Correo_Tecnico { get; set; }
        public string Descripcion { get; set; }
    }
    public class TecnicoViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }

        // Propiedad para mostrar en el select
        public string NombreCompleto => $"{Nombre} {Apellido} ({Correo})";
    }

    public class ClienteViewModel
    {
        public string NOMBRECLIENTE { get; set; }
        public string APELLIDOCLIENTE { get; set; }
        public string CORREOCLIENTE { get; set; }
        public string DIRECCIONCLIENTE { get; set; }
        public string NOMBREEMPRESA { get; set; }

        // Propiedad para mostrar en el select
        public string NombreCompleto => $"{NOMBRECLIENTE} {APELLIDOCLIENTE} - {NOMBREEMPRESA} ({CORREOCLIENTE})";
    }

    public class FormularioTicketViewModel
    {
        public CrearTicket CrearTicket { get; set; }
        public List<TecnicoViewModel> Tecnicos { get; set; }
        public List<ClienteViewModel> Clientes { get; set; }
    }
}

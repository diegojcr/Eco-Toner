using System.ComponentModel.DataAnnotations;

namespace Eco_Toner.Models
{
public class IngresoCliente
    {
        // Propiedades del cliente
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }

        // Para selección
        public string EmpresaSeleccionada { get; set; }
        public string OficinaSeleccionada { get; set; }

        // Listas para dropdowns
        public List<string> Empresas { get; set; } = new List<string>();
        public List<string> Oficinas { get; set; } = new List<string>();

    }
}
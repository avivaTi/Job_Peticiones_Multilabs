using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviva.Multilabs.Peticiones.Demonio.Data
{
    public class Lista_Peticiones_Multilabs
    {
        public string Numero_de_orden { get; set; }
        public string Estado { get; set; }
        public int encuentro { get; set; }
        public int codigo_cliente { get; set; }
        public string Historia_clinica { get; set; }
        public string Paciente { get; set; }
        public string DNI { get; set; }
        public  string Sexo { get; set; }
        public int Edad { get; set; }
        public string Procedimiento { get; set; }
        public string Servicio { get; set; }
        public string Ubicacion { get; set; }
        public string Prioridad { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Tiempo_de_Espera { get; set; }
        public int Peticion { get; set; }
        public int Catalogo { get; set; }
        public string Estado_Muestra { get; set; }
        public string Comentarios { get; set; }
        public int SedeId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviva.Multilabs.Peticiones.Demonio.Models
{
    public class General
    {
        public string fecha_atencion { get; set; }
        public string cod_atencion { get; set; }
        public string cod_externo { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string fecha_nacimiento { get; set; }
        public string genero { get; set; }
        public string tipo_doc { get; set; }
        public string doc_identidad { get; set; }
        public string fecha_publicacion { get; set; }
        public string url_pdf { get; set; }
        public string estado_lab { get; set; }
        public int examenes_total { get; set; }
        public int examenes_completados { get; set; }
        public string cmp_solicitante { get; set; }
        public string doctor_solicitante { get; set; }
        public string fecha_toma_muestra { get; set; }
    }

    public class Detalle
    {
        public string codExamen { get; set; }
        public string codParametro { get; set; }
        public string examenes { get; set; }
        public string rangos { get; set; }
        public string unidades { get; set; }
        public string resultado_completo { get; set; }
        public string resultado { get; set; }
        public int fuera_de_rango { get; set; }
        public int es_critico { get; set; }
        public int es_sensible { get; set; }
        public string comentario { get; set; }
        public string metodo { get; set; }
    }

    public class MultilabsResponse
    {
        public List<General> general { get; set; }
        public List<Detalle> detalle { get; set; }
    }
}

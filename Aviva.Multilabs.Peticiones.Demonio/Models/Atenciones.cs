using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviva.Multilabs.Peticiones.Demonio.Models
{
    public class Atenciones
    {
        public string fecha_atencion { get; set; }
        public string cod_atencion { get; set; }
        public string cod_externo { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string tipo_doc { get; set; }
        public string doc_identidad { get; set; }
        public string fecha_publicacion { get; set; }
        public string url_pdf { get; set; }
        public string estado_lab { get; set; }
        public int examenes_total { get; set; }
        public int examenes_completados { get; set; }
        public string fecha_toma_muestra { get; set; }
    }

    public class Root
    {
        public List<Atenciones> atenciones { get; set; }
    }
}

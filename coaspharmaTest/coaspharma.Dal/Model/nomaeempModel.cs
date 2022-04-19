using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coaspharma.Dal.Model
{
   public class nomaeempModel
    {
        public string codcia { get; set; }
        public string cedula { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nombrecompleto { get; set; }
        public string fecha_ingreso { get; set; }
        public string dep_codi { get; set; }
        public string mun_codi { get; set; }
        public byte[] foto { get; set; }
        public string ubicacion { get; set; }
       
    }
}

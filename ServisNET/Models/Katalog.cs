using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServisNET.Models
{
    public class Katalog
    {
        public int Id { get; set; }
        public string ParcaKodu { get; set; }
        public string Ad { get; set; }
        public string IslemTipi { get; set; }
        public double StokAdedi { get; set; }
        public double RezerveAdet { get; set; }
        public double BirimFiyat { get; set; }
    }
}

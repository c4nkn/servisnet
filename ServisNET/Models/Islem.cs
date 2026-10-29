using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServisNET.Models
{
    public class Islem
    {
        public int Id { get; set; }
        public int KayitId { get; set; }
        public int KatalogId { get; set; }
        public string IslemTipi { get; set; }
        public string Aciklama { get; set; }
        public double Miktar { get; set; }
        public double BirimFiyat { get; set; }
    }
}

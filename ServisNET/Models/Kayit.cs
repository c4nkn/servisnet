using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServisNET.Models
{
    public class Kayit
    {
        public int Id { get; set; }
        public int AracId { get; set; }
        public string GirisTarihi { get; set; }
        public string CikisTarihi { get; set; }
        public int GuncelKilometre { get; set; }
        public string ArizaKodu { get; set; }
        public string MusteriSikayeti { get; set; }
        public string TeknisyenNotu { get; set; }
        public double ToplamTutar { get; set; }
        public string Durum { get; set; }
    }
}

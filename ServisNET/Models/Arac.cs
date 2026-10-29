using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServisNET.Models
{
    public class Arac
    {
        public int Id { get; set; }
        public string SasiNo { get; set; }
        public string Plaka { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public int Yil { get; set; }
        public string MusteriAdSoyad { get; set; }
        public string MusteriTelefon { get; set; }
        public int SonKilometre { get; set; }
    }
}

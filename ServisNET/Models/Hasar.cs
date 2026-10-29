using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServisNET.Models
{
    internal class Hasar
    {
        public int Id { get; set; }
        public int KayitId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string HasarTipi { get; set; }
        public string Aciklama { get; set; }
    }
}

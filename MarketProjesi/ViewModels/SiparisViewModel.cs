using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketProjesi.ViewModels
{

    public class SiparisViewModel
    {
        public int SiparisNo { get; set; }
        public DateTime SiparisTarihi { get; set; }
        public DateTime? TeslimTarihi { get; set; }
        public decimal KargoTutari { get; set; }
        public decimal ToplamSiparisTutari { get; set; }
    }
}

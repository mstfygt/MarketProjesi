using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketProjesi.ViewModels
{
    class UrunviewModel
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public decimal? Fiyat { get; set; }
        public short Stok { get; set; }
        public string KategoriAdi { get; set; }
        public override string ToString() => $"{KategoriAdi} {UrunAdi}: {Fiyat:c2} - Kalan: {Stok}";
    }
}

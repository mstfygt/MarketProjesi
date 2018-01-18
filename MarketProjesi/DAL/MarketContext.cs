using MarketProjesi.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketProjesi.DAL
{
   public class MarketContext : DbContext
    {
        public MarketContext () : base("name=MarCon")
        {
        }
        public virtual DbSet<Kategori> Kategoriler { get; set; }
        public virtual DbSet<Urun> Urunler { get; set; }
        public virtual DbSet<SiparisDetay> SiparisDetaylar { get; set; }
        public virtual DbSet<Siparis> Siparisler { get; set; }
    }
}

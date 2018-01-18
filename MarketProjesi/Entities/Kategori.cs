using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MarketProjesi.Entities
{
    [Table("Kategoriler")]
    public  class Kategori
    {
        [Key]
        public int KategoriId { get; set; }
        [Required]
        [StringLength(25)]
        [Column("Kategori Adı", TypeName = "varchar")]
        public string KategoriAdi { get; set; }
        public string Aciklama { get; set; }
        public decimal KDVOran { get; set; } = 0.18m;


        public virtual List<Urun> Urunler { get; set; } = new List<Urun>();
    }
}

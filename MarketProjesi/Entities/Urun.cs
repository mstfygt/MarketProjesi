using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketProjesi.Entities
{
    [Table("Ürünler")]
   public  class Urun
    {
        [Key]
        public int UrunId { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "Ürün adı 2-40 karakter aralığında olmalıdır")]
        [Index(IsUnique = true)]
        public string UrunAdi { get; set; }
        public decimal Fiyat { get; set; } = 0;
        public short Stok { get; set; } = 0;
        public long SeriNo { get; set; }
        public byte[] Fotograf { get; set; }
        //  public DateTime EklenmeZamani { get; set; } = DateTime.Now;
        public int KategoriId { get; set; }
        [ForeignKey("KategoriId")]
        public virtual Kategori Kategori { get; set; }
    }
}

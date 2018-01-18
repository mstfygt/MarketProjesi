using MarketProjesi.DAL;
using MarketProjesi.Entities;
using MarketProjesi.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketProjesi.Forms
{
    public partial class FormSiparisOlustur : Form
    {
        public FormSiparisOlustur()
        {
            InitializeComponent();
           
        }
        List<UrunviewModel> urunList = new List<UrunviewModel>();
        List<SepetViewModel> sepetList = new List<SepetViewModel>();
        private void FormSiparis_Load(object sender, EventArgs e)
        {
            VerileriGetir();
        }

        private void VerileriGetir(string arama = "")
        {
            try
            {
                var ara = arama.ToLower();
                MarketContext db = new MarketContext();
                var sonuc = from prod in db.Urunler
                                //join cat in db.Categories on prod.CategoryID equals cat.CategoryID
                            where (prod.UrunAdi.ToLower().Contains(ara) || prod.Kategori.KategoriAdi.Contains(ara))
                            orderby prod.UrunAdi ascending
                            select new UrunviewModel
                            {
                                UrunId = prod.UrunId,
                                UrunAdi = prod.UrunAdi,
                                Fiyat = prod.Fiyat,
                                Stok = prod.Stok,
                                KategoriAdi = prod.Kategori.KategoriAdi
                            };
                lstUrun.DataSource = sonuc.ToList();
                urunList = sonuc.ToList();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        bool StokKontrol(UrunviewModel seciliUrun, short adet = 1)
        {
            bool varmi = false, cevap = false;
            sepetList.ForEach(x =>
            {
                if (x.UrunId != seciliUrun.UrunId) return;
                varmi = true;
                if (adet == 1)
                {
                    cevap = seciliUrun.Stok > x.Adet;
                }
                else if (adet > 1)
                {
                    cevap = seciliUrun.Stok >= adet;
                }
            });

            if (!varmi)
            {
                cevap = seciliUrun.Stok > 0;
            }
            return cevap;
        }
        void SepetGuncelle()
        {
            lstSepet.Items.Clear();
            sepetList.ForEach(x => lstSepet.Items.Add(x));
            SepetHesapla();
        }
        void SepetHesapla()
        {
            decimal toplam = 0, kdv = 0;
            toplam = sepetList.Sum(x => x.Toplam);
            kdv = toplam * 0.18m;
            nToplam.Value = toplam;
            nKdv.Value = kdv;
        }
        void Temizle()
        {
            sepetList = new List<SepetViewModel>();
            lstSepet.Items.Clear();
            VerileriGetir();
        }

        private void btnSepeteEkle_Click(object sender, EventArgs e)
        {
            if (lstUrun.SelectedItem == null) return;
            var seciliUrun = lstUrun.SelectedItem as UrunviewModel;
            bool varmi = false;
            sepetList.ForEach(x =>
            {
                if (x.UrunId == seciliUrun.UrunId)
                {
                    varmi = true;
                    x.Adet++;
                }
            });
            if (!varmi)
            {
                var model = new SepetViewModel()
                {
                    UrunId = seciliUrun.UrunId,
                    Indirim = nIndirim.Value,
                    UrunAdi = seciliUrun.UrunAdi,
                    Fiyat = seciliUrun.Fiyat ?? 0
                };
                sepetList.Add(model);
            }
            SepetGuncelle();
        }

        private void btnSiparisOlustur_Click(object sender, EventArgs e)
        {
            if (sepetList.Count == 0)
            {
                MessageBox.Show("Önce Sepete Ürün Eklemelisiniz");
                return;
            }

           
            string mesaj = $"{ nToplam.Value :c2} tutarındaki siparişi onaylıyor musunuz?";
            var cevap = MessageBox.Show(mesaj, "Sipariş Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cevap != DialogResult.Yes) return;
            MarketContext db = new MarketContext();
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var yeniSiparis = new Siparis()
                    {
                        SiparisTarihi = ye
                    };
                    db.Siparisler.Add(yeniSiparis);
                    db.SaveChanges();
                    foreach (var item in sepetList)
                    {
                        var siparisDetay = new SiparisDetay()
                        {
                            SiparisId = yeniSiparis.SiparisId,
                            UrunId = item.UrunId,
                            Adet = item.Adet,
                            Fiyat = item.Fiyat,
                            Indirim = item.Indirim
                        };
                        db.SiparisDetaylar.Add(siparisDetay);
                    }
                    db.SaveChanges();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}

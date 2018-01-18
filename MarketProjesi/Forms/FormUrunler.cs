using MarketProjesi.DAL;
using MarketProjesi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketProjesi.Forms
{
    public partial class FormUrunler : Form
    {
        public FormUrunler()
        {
            InitializeComponent();
        }

        private void FormUrunler_Load(object sender, EventArgs e)
        {
            VerileriGetir();
        }

        private void VerileriGetir()
        {
            try
            {
                MarketContext db = new MarketContext();
                cmbKategori.DataSource = db.Kategoriler.ToList();
                cmbKategori.DisplayMember = "KategoriAdi";
                cmbKategori.ValueMember = "KategoriId";

                lstUrun.DataSource = db.Urunler.OrderBy(x => x.UrunAdi).ToList();
                lstUrun.DisplayMember = "UrunAdi";
                lstUrun.ValueMember = "UrunId";

                this.Text = $" Toplam Ürün Sayısı: {db.Urunler.Count()} Toplam Ürün Maliyeti: {db.Urunler.Sum(x => x.Fiyat* x.Stok):c2} - {DateTime.Now:dd MMMM yy dddd}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            Urun urun = new Urun()
            {
                Fiyat = nFiyat.Value,
              
                Stok = Convert.ToInt16(nStok.Value),
                KategoriId = (int)cmbKategori.SelectedValue,
                UrunAdi = txtUrunAdi.Text,
                 SeriNo = long.Parse( txtSeriNo.Text)

            };
            try
            {
                MarketContext db = new MarketContext();
                db.Urunler.Add(urun);
                db.SaveChanges();
                VerileriGetir();
                lstUrun.SelectedValue = urun.UrunId;
            }
            catch (DbEntityValidationException ex)
            {
                string mesajlar = string.Empty;
                foreach (var ev in ex.EntityValidationErrors)
                {
                    foreach (var ve in ev.ValidationErrors)
                    {
                        mesajlar += $"{ve.PropertyName} - {ve.ErrorMessage}\n";
                    }
                }
                MessageBox.Show(mesajlar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (lstUrun.SelectedItem == null) return;
            var seciliUrun = lstUrun.SelectedItem as Urun;
            try
            {
                MarketContext db = new MarketContext();
                seciliUrun = db.Urunler.Find(seciliUrun.UrunId);
                if (seciliUrun == null)
                {
                    MessageBox.Show("Ürün Bulunamadı");
                    VerileriGetir();
                    return;
                }
                seciliUrun.UrunAdi = txtUrunAdi.Text;
                seciliUrun.Fiyat = nFiyat.Value;
               
                seciliUrun.Stok = Convert.ToInt16(nStok.Value);
                seciliUrun.KategoriId = (int)cmbKategori.SelectedValue;
                seciliUrun.SeriNo = long.Parse(txtSeriNo.Text);
                db.SaveChanges();
                VerileriGetir();
                lstUrun.SelectedValue = seciliUrun.UrunId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (lstUrun.SelectedItem == null) return;
            var seciliUrun = lstUrun.SelectedItem as Urun;
            var cevap = MessageBox.Show($"{seciliUrun.UrunAdi} isimli ürünü silmek istiyor musunuz?", "Ürün Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cevap != DialogResult.Yes) return;
            try
            {
                MarketContext db = new MarketContext();
                seciliUrun = db.Urunler.Find(seciliUrun.UrunId);
                if (seciliUrun == null)
                {
                    MessageBox.Show("Ürün Bulunamadı");
                    VerileriGetir();
                    return;
                }
                db.Urunler.Remove(seciliUrun);
                db.SaveChanges();
                VerileriGetir();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private byte[] resimDosyasi;
        private void pibUrunResim_Click(object sender, EventArgs e)
        {
            OpenFileDialog dosyaAc = new OpenFileDialog
            {
                Title = "Bir resim dosyası seçiniz",
                Multiselect = false,
                Filter = "JPG Formatı (*.jpg) | *.jpg; *.jpeg; | PNG Formatı | *.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            DialogResult result = dosyaAc.ShowDialog();
            MemoryStream memoryStream = new MemoryStream();
            var buffer = new byte[64];
            if (result == DialogResult.OK)
            {
                FileStream fileStream = File.Open(dosyaAc.FileName, FileMode.Open);
                while (fileStream.Read(buffer, 0, 64) != 0)
                {
                    memoryStream.Write(buffer, 0, 64);
                }
                resimDosyasi = memoryStream.ToArray();
               pbUrunResim.Image = new Bitmap(memoryStream);
            }
        }

        private void lstUrun_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstUrun.SelectedItem == null) return;
            var seciliUrun = lstUrun.SelectedItem as Urun;
            txtUrunAdi.Text = seciliUrun.UrunAdi;
            nFiyat.Value = seciliUrun.Fiyat;
           
            cmbKategori.SelectedItem = seciliUrun.Kategori;
            nStok.Value = seciliUrun.Stok;
            txtSeriNo.Text = seciliUrun.SeriNo.ToString();
        }

        private void txtSeriNo_KeyDown(object sender, KeyEventArgs e)
        {
            
            
            if (e.KeyCode == Keys.Return)
            {
                txtSeriNo.Clear();
                e.Handled = true;
                VerileriGetir(); // Gridi doldurmak için yazdığın metod
            }
        }

       
    }
}

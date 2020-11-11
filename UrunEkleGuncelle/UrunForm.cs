using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UrunEkleGuncelle.Models;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;

namespace UrunEkleGuncelle
{
    public partial class UrunForm : Form
    {
        BindingList<Urun> blUrunler = new BindingList<Urun>();
        public UrunForm()
        {
            VerileriOku();
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;  //otomatik sütun oluşturmayı kapattık.
            dgvUrunler.DataSource = blUrunler;
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            if (dgvUrunler.SelectedRows.Count == 0)
            {
                return;
            }
            Urun seciliUrun = (Urun)dgvUrunler.SelectedRows[0].DataBoundItem;
            DuzenleForm frmDuzenle = new DuzenleForm(seciliUrun);
            frmDuzenle.UrunDuzenlendi += FrmDuzenle_UrunDuzenlendi;
            frmDuzenle.ShowDialog();
        }

        private void FrmDuzenle_UrunDuzenlendi(object sender, EventArgs e)
        {
            blUrunler.ResetBindings();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            var urunAd = txtUrunAd.Text.Trim();

            if (urunAd == "")
            {
                MessageBox.Show("Ürün Adı Girmediniz");
                return;
            }
            blUrunler.Add(new Urun { UrunAd = urunAd, BirimFiyat = nudBirimFiyat.Value });
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvUrunler.SelectedRows.Count == 0)
            {
                return;
            }
            Urun seciliUrun = (Urun)dgvUrunler.SelectedRows[0].DataBoundItem;
            blUrunler.Remove(seciliUrun);
        }

        private void dgvUrunler_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                btnDuzenle.PerformClick();
            }
        }

        private void dgvUrunler_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUrunler.SelectedRows.Count > 0)
            {
                btnDuzenle.Enabled = btnSil.Enabled = true;
            }
            else
                btnDuzenle.Enabled = btnSil.Enabled = false;
        }

        private void dgvUrunler_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvUrunler.SelectedRows.Count > 0 && e.KeyCode == Keys.Delete)
            {
                btnSil.PerformClick();
            }
        }

        private void UrunForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            VerileriKaydet();
        }

        private void VerileriKaydet()
        {
            string json = JsonConvert.SerializeObject(blUrunler);
            File.WriteAllText("veri.json", json);
        }
        private void VerileriOku()
        {
            try
            {
                string json = File.ReadAllText("veri.json");
                blUrunler = JsonConvert.DeserializeObject<BindingList<Urun>>(json);
            }
            catch (Exception)
            {

                OrnekVerileriYukle();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrunEkleGuncelle.Models
{
    public class Urun
    {
        public string UrunAd { get; set; }
        public decimal BirimFiyat { get; set; }
        public string BirimFiyatTL => $"{BirimFiyat:0.00}₺";

        public override string ToString()
        {
            return string.Format("{0} ({1:0.00)}", UrunAd, BirimFiyat);
        }
    }
}

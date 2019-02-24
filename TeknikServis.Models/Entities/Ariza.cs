using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Abstract;
using TeknikServis.Models.Enums;

namespace TeknikServis.Models.Entities
{
    [Table("Arizalar")]
  public   class Ariza:BaseEntity<int>
    {

        [Required]
        [DisplayName("Muşteri Adı")]
        public string MusteriId { get; set; }
        [DisplayName("Operatör Adı")]
        public string  OperatorId{ get; set; }
        [DisplayName("Teknisyen Adı")]
        public string TeknisyenId { get; set; }
        [Required]
        [DisplayName("Arıza Oluşturma Tarihi")]
        public DateTime ArizaOlusturmaTarihi { get; set; } = DateTime.Now;
     
        [DisplayName("Arıza Başlanma Tarihi")]
        public DateTime? ArizaKabulTarihi { get; set; }
       
        [DisplayName("Arıza Tamir Bitiş Tarihi")]
        public DateTime? ArizaBitisTarihi { get; set; }
       
        [DisplayName("Tamir Durumu")]
        public bool? TamirEdildiMi { get; set; }
    
        [DisplayName("Arıza Kabul Durumu")]
        public bool? ArizaKabulEdildiMi { get; set; }

        [DisplayName("Arıza Teknisyen Durumu")]
        public bool? ArizaTeknisyeneAtandiMi { get; set; }
        [Required]
        [DisplayName("Garanti Durumu")]
        public bool GarantiDurumu { get; set; }

        [Required]
        [DisplayName("Arıza Açıklaması")]
        public string MusteriYorumu { get; set; }

        [Required]
        [DisplayName("Ürün Adı")]
        public string UrunAdi { get; set; }

        [DisplayName("Tamir Açıklaması")]
        public string TeknisyenYorumu { get; set; }
       
        [DisplayName("Fatura Fotoğrafı")]
        public string FaturaResmi { get; set; }
        [Required]
        [DisplayName("Ürün Resmi")]
        public string UrunResmi { get; set; }
        [Required]
        [DisplayName("Ürün Adresi")]
        public string Adres { get; set; }

        [DisplayName("Ürün Durumu")]
        public UrunDurumu? UrunDurumu { get; set; }

        [Required]
        [DisplayName("Ürün Türü")]
        public UrunTipi UrunTipi { get; set; }
        [Required]
        [DisplayName("Şehir Adı")]
        public SehirAdi SehirAdi { get; set; }



    }
}

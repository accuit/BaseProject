using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccuIT.PresentationLayer.WebAdmin;
using System.ComponentModel.DataAnnotations;

namespace AccuIT.PresentationLayer.WebAdmin.ViewDataModel
{
    public class QuotationViewModel
    {
        [Key]
        public int caseid { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? date { get; set; }
        [Required]
        public string employee { get; set; }

        public string calling { get; set; }
        public string deal { get; set; }
        [Required]
        public string company { get; set; }

        public string contactperson { get; set; }
        public string address { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
        [Required]
        public string mobile { get; set; }
        [Required]
        public string email { get; set; }
        public string altemail { get; set; }
        public string Source { get; set; }
        public string commmode { get; set; }
        [Required]
        public string catg { get; set; }
        [Required]
        public string prod { get; set; }
        public string quantity { get; set; }
        public string action { get; set; }
        public string sale { get; set; }

        public double? payable { get; set; }
        public double? received { get; set; }
        public double? due { get; set; }
        public string remarks { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? lastupdate { get; set; }


        public int SerialNo { get; set; }
        public string ProdName { get; set; }
        public int? Quantity { get; set; }
        public string Unit { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? NetPrice { get; set; }
    }
}
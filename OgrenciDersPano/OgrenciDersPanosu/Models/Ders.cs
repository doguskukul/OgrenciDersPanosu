using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    public class Ders
    {
        //[ForeignKey("Ogretmen")]

        [Key]
        public string DersId { get; set; }

        public string DersAdi { get; set; }


        public OgretmenModel Ogretmen { get; set; }
        public IEnumerable<Not> Notlar {get;set;}


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    public class Not
    {

        //[ForeignKey("Ders")]
       
        public Ders Ders { get; set; }

        //[ForeignKey("Ogrenci")]

        public Ogrenci Ogrenci { get; set; }

        public int Sinav1 { get; set; }

        public int Sinav2 { get; set; }

        public int Sinav3 { get; set; }

        public int Sozlu1 { get; set; }

        public int Sozlu2 { get; set; }

        public int Sozlu3 { get; set; }

        public string NotId { get; set; }

    }
}
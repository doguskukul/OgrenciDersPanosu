using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.Models
{
    public class Ders
    {
        //[ForeignKey("Ogretmen")]

        public string DersId { get; set; }

        public string DersAdi { get; set; }

        
        public Ogretmen Ogretmen { get; set; }


    }
}
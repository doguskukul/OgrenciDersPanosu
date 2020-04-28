using Microsoft.AspNet.Identity.EntityFramework;
using OgrenciDersPanosu.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OgrenciDersPanosu.identity
{
    public class IdentityDataContext:IdentityDbContext<ApplicationUser>
    {
        public IdentityDataContext() : base("identityConnection")
        {

        }


        public DbSet<Not> Notlar { get; set; } 

        public DbSet<Ogrenci> Ogrenciler { get; set; }

        public DbSet<Ogretmen> Ogretmenler { get; set; }

        public DbSet<Ders> Dersler { get; set; }


    }
}
namespace OgrenciDersPanosu.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OgrenciDersPanosu.identity.IdentityDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "OgrenciDersPanosu.identity.IdentityDataContext";
        }

        protected override void Seed(OgrenciDersPanosu.identity.IdentityDataContext context)
        {
            context.Ogrenciler.AddOrUpdate(new Models.Ogrenci { OgrenciId = "1234", Ad = "Bahadir" , Soyad = "Gultekin"} ) ;

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}

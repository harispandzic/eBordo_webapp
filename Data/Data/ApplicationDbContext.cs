using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.EntityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eBordo.Data
{
    public class ApplicationDbContext : IdentityDbContext<Korisnik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
               .SelectMany(t => t.GetForeignKeys())
               .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Korisnik>()
                .HasOne<Igrac>(s => s.igrac)
                .WithOne(ad => ad.korisnik)
                .HasForeignKey<Igrac>(ad => ad.korisnikID)
                ;

            modelBuilder.Entity<Korisnik>()
                .HasOne<Trener>(s => s.trener)
                .WithOne(ad => ad.korisnik)
                .HasForeignKey<Trener>(ad => ad.korisnikID)
                ;

            modelBuilder.Entity<IgracStatistika>()
                .HasOne<Igrac>(s => s.igrac)
                .WithOne(ad => ad.igracStatistika)
                .HasForeignKey<Igrac>(ad => ad.igracStatistikaID)
                ;

            modelBuilder.Entity<IgracSkills>()
                .HasOne<Igrac>(s => s.igrac)
                .WithOne(ad => ad.igracSkills)
                .HasForeignKey<Igrac>(ad => ad.igracSkillsID)
                ;

            modelBuilder.Entity<Ugovor>()
                .HasOne<Igrac>(s => s.igrac)
                .WithOne(ad => ad.ugovor)
                .HasForeignKey<Igrac>(ad => ad.ugovorID)
                ;

            modelBuilder.Entity<Ugovor>()
                .HasOne<Trener>(s => s.trener)
                .WithOne(ad => ad.ugovor)
                .HasForeignKey<Trener>(ad => ad.ugovorID)
                ;

            modelBuilder.Entity<PrvaPostava>()
               .HasOne<Utakmica>(s => s.utakmica)
               .WithOne(ad => ad.prvaPostava)
               .HasForeignKey<Utakmica>(ad => ad.utakmicaID)
               ;

            modelBuilder.Entity<Klupa>()
               .HasOne<Utakmica>(s => s.utakmica)
               .WithOne(ad => ad.klupa)
               .HasForeignKey<Utakmica>(ad => ad.utakmicaID)
               ;

            modelBuilder.Entity<TrenerStatistika>()
                .HasOne<Trener>(s => s.trener)
                .WithOne(ad => ad.trenerStatistika)
                .HasForeignKey<Trener>(ad => ad.trenerStatistikaID)
                ;
        }

        public DbSet<Drzava> drzave { get; set; }
        public DbSet<Grad> gradovi { get; set; }
        public DbSet<Klub> klubovi { get; set; }
        public DbSet<VrstaTakmicenja> vrsteTakmicenja { get; set; }
        public DbSet<IgracSkills> igracSkills { get; set; }
        public DbSet<IgracStatistika> igracStatistika { get; set; }
        public DbSet<SkillsStavke> skillsStavke { get; set; }
        public DbSet<Ugovor> ugovori { get; set; }
        public DbSet<Utakmica> utakmice { get; set; }
        public DbSet<UtakmicaNastupi> nastupi { get; set; }
        public DbSet<UtakmicaOcjene> ocjene { get; set; }
        public DbSet<PrvaPostava> prvaPostava { get; set; }
        public DbSet<Klupa> klupa { get; set; }
        public DbSet<GarnituraDresova> garnitureDresova { get; set; }
        public DbSet<Stadion> stadioni { get; set; }
        public DbSet<Izvještaj> izvjestaji { get; set; }
        public DbSet<Korisnik> korisnici { get; set; }
        public DbSet<Igrac> igraci { get; set; }
        public DbSet<Trener> treneri { get; set; }
        public DbSet<TrenerStatistika> trenerStatistika { get; set; }
        public DbSet<Trening> treninzi { get; set; }
        public DbSet<Notifikacija> notifikacije { get; set; }
        public DbSet<Zahtjev> zahtjevi { get; set; }

        public DbSet<Izmjena> izmjena { get; set; }
        public DbSet<UtakmicaIzmjena> utakmicaIzmjena { get; set; }
        public DbSet<Povreda> povrede { get; set; }
    }
}

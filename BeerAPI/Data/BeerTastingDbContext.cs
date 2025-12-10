using BeerAPI.Models;
using BeerAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BeerTasting.Api.Data;

public class BeerTastingDbContext(DbContextOptions<BeerTastingDbContext> options) : DbContext(options)
{
    public DbSet<Beer> Beers => Set<Beer>();
    public DbSet<Tasting> Tastings => Set<Tasting>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Rating> Ratings => Set<Rating>();

    public DbSet<DesignRanking> DesignRankings => Set<DesignRanking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Tasting
        modelBuilder.Entity<Tasting>()
            .HasMany(t => t.BeerList)
            .WithOne(b => b.Tasting!)
            .HasForeignKey(b => b.TastingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tasting>()
            .HasMany(t => t.Participants)
            .WithOne(p => p.Tasting!)
            .HasForeignKey(p => p.TastingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tasting>()
            .HasMany(t => t.Ratings)
            .WithOne(r => r.Tasting!)
            .HasForeignKey(r => r.TastingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tasting>()
            .HasMany(t => t.DesignRankings)
            .WithOne(dr => dr.Tasting!)
            .HasForeignKey(dr => dr.TastingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DesignRanking>()
            .HasIndex(dr => new { dr.TastingId, dr.ParticipantId })
            .IsUnique();

        // Rating uniqueness: one rating per participant/beer/tasting
        modelBuilder.Entity<Rating>()
            .HasIndex(r => new { r.TastingId, r.ParticipantId, r.BeerId })
            .IsUnique();

        // Seed beer catalog (like the mock)
        modelBuilder.Entity<Beer>().HasData(
            new Beer { Id = "1", Name = "Ægir Jólalager", Brewery = "Ægir Brugghús ehf.", Style = "Christmas Ale", Abv = 5.2, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "2", Name = "Ákaflega gaman þá Double IPA", Brewery = "RVK Bruggfélag", Style = "IPA", Abv = 7.5, VolumeMl = 440, IsCatalogBeer = true },
            new Beer { Id = "3", Name = "Albani Jule Bryg Blaa Lys", Brewery = "Royal Unibrew A/S", Style = "Ale", Abv = 7.0, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "4", Name = "Antichrist Imperial Stout", Brewery = "Lady Brewery ehf.", Style = "Imperial Stout", Abv = 10.5, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "5", Name = "Askasleikir nr. 45 Amber Ale", Brewery = "Borg Brugghús", Style = "Ale", Abv = 5.4, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "6", Name = "Áttavittur nr. 116 Double Bock", Brewery = "Borg Brugghús", Style = "Ale", Abv = 6.7, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "7", Name = "Be Merry Me Berry", Brewery = "Lady Brewery ehf.", Style = "Ale", Abv = 6.5, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "8", Name = "Boli X Mas", Brewery = "Ölgerðin Egill Skallagrímsson", Style = "Ale", Abv = 5.0, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "9", Name = "Brió 0%", Brewery = "Borg", Style = "Non-Alcoholic", Abv = 0.0, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "10", Name = "Corsendonk Christmas Ale", Brewery = "Corsendonk", Style = "Christmas Ale", Abv = 8.5, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "11", Name = "Dagsson jólabjór með bubblum", Brewery = "Gæðingur-Öl ehf", Style = "Christmas Ale", Abv = 5.6, VolumeMl = 330, IsCatalogBeer = true },
            new Beer { Id = "12", Name = "Einstök Winter Ale", Brewery = "Einstök Ölgerð", Style = "Winter Ale", Abv = 6.0, VolumeMl = 330, IsCatalogBeer = true }
        );
    }
}

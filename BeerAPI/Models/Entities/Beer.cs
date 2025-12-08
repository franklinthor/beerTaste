namespace BeerAPI.Models;

public class Beer
{
    // String IDs keep it simple and align with the frontend UUID strings
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public string Brewery { get; set; } = null!;
    public string Style { get; set; } = null!;
    public double Abv { get; set; }
    public int VolumeMl { get; set; }

    // Catalog vs per-tasting snapshot
    public bool IsCatalogBeer { get; set; }

    // If this beer belongs to a specific tasting (snapshot)
    public string? TastingId { get; set; }
    public Tasting? Tasting { get; set; }
}
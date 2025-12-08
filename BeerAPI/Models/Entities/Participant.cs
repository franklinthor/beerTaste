namespace BeerAPI.Models.Entities;

public class Participant
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;

    public string TastingId { get; set; } = null!;
    public Tasting? Tasting { get; set; }
}
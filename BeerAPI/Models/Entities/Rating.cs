namespace BeerAPI.Models.Entities;

public class Rating
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string TastingId { get; set; } = null!;
    public Tasting? Tasting { get; set; }

    public string ParticipantId { get; set; } = null!;
    public Participant? Participant { get; set; }

    public string BeerId { get; set; } = null!;
    public Beer? Beer { get; set; }

    public int Score { get; set; }  // 1–5
    public string? Comment { get; set; }
}
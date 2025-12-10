namespace BeerAPI.Models.Entities;

public class DesignRanking
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string TastingId { get; set; } = null!;
    public Tasting? Tasting { get; set; }

    public string ParticipantId { get; set; } = null!;
    public Participant? Participant { get; set; }

    /// <summary>
    /// Ordered list of beer IDs (most appealing -> least)
    /// Stored as JSON string: ["beerId1","beerId2",...]
    /// </summary>
    public string RankingsJson { get; set; } = "[]";
}
namespace BeerAPI.Models.Entities;

public enum TastingStatus
{
    Pending = 0,
    Active = 1,
    DesignRound = 2,
    Finished = 3
}

public class Tasting
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string HostName { get; set; } = null!;
    public bool IsBlind { get; set; }
    public int CurrentBeerIndex { get; set; } = 0;
    public TastingStatus Status { get; set; } = TastingStatus.Pending;

    // NEW: design round options
    public bool DesignRoundEnabled { get; set; } = false;
    public bool DesignRoundStarted { get; set; } = false;

    public ICollection<Beer> BeerList { get; set; } = new List<Beer>();
    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    // NEW: design rankings from participants
    public ICollection<DesignRanking> DesignRankings { get; set; } = new List<DesignRanking>();
}
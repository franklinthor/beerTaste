namespace BeerAPI.Models.Entities;

public enum TastingStatus
{
    Pending,
    Active,
    Finished
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

    public ICollection<Beer> BeerList { get; set; } = new List<Beer>();
    public ICollection<Participant> Participants { get; set; } = new List<Participant>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
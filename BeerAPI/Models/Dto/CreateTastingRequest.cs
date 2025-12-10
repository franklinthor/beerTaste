namespace BeerAPI.Models.Dto;

public record CreateTastingRequest(
    string Name,
    string HostName,
    bool IsBlind,
    bool HasDesignRound,
    IReadOnlyList<BeerDto> Beers
);
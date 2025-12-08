namespace BeerAPI.Models.Dto;

public record CreateTastingRequest(
    string Name,
    string HostName,
    bool IsBlind,
    IReadOnlyList<BeerDto> Beers
);
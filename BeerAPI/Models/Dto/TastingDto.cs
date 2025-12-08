namespace BeerAPI.Models.Dto;

public record TastingDto(
    string Id,
    string Code,
    string Name,
    string HostName,
    bool IsBlind,
    IReadOnlyList<BeerDto> Beers,
    int CurrentBeerIndex,
    string Status,
    IReadOnlyList<ParticipantDto> Participants,
    IReadOnlyList<RatingDto> Ratings
);
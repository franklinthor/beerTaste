namespace BeerAPI.Models.Dto;

public record DesignRankingDto(
    string ParticipantId,
    IReadOnlyList<string> Rankings
);

public record TastingDto(
    string Id,
    string Code,
    string Name,
    string HostName,
    bool IsBlind,
    bool HasDesignRound,
    IReadOnlyList<BeerDto> Beers,
    int CurrentBeerIndex,
    string Status,
    IReadOnlyList<ParticipantDto> Participants,
    IReadOnlyList<RatingDto> Ratings,
    IReadOnlyList<DesignRankingDto> DesignRankings
);
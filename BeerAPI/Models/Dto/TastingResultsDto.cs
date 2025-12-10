namespace BeerAPI.Models.Dto;

public record DistributionBucketDto(
    int Score,
    int Count
);

public record BeerResultDto(
    BeerDto Beer,
    double AverageScore,
    int RatingCount,
    IReadOnlyList<DistributionBucketDto> Distribution,
    int DesignPoints,
    double CombinedScore
);

public record TastingResultsDto(
    IReadOnlyList<BeerResultDto> Beers
);
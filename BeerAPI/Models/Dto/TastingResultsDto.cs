namespace BeerAPI.Models.Dto;

public record DistributionBucketDto(
    int Score,
    int Count
);

public record BeerResultDto(
    BeerDto Beer,
    double AverageScore,
    int RatingCount,
    IReadOnlyList<DistributionBucketDto> Distribution
);

public record TastingResultsDto(
    IReadOnlyList<BeerResultDto> Beers
);
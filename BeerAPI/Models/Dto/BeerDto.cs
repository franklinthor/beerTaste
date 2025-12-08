namespace BeerAPI.Models.Dto;

public record BeerDto(
    string Id,
    string Name,
    string Brewery,
    string Style,
    double Abv,
    int VolumeMl
);
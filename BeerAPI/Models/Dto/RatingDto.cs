namespace BeerAPI.Models.Dto;

public record RatingDto(
    string ParticipantId,
    string BeerId,
    int Score,
    string? Comment
);
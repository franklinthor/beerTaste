namespace BeerAPI.Models.Dto;

public record SubmitRatingRequest(
    string BeerId,
    string ParticipantId,
    int Score,
    string? Comment
);
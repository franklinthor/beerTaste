namespace BeerAPI.Models.Dto;

public record SubmitDesignRankingRequest(
    string ParticipantId,
    IReadOnlyList<string> Rankings
);
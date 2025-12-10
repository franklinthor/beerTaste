using System.Text.Json;
using BeerAPI.Models.Dto;
using BeerAPI.Models.Entities;

namespace BeerAPI.Helpers;

public static class TastingMappings
{
    public static BeerDto ToDto(this Beer beer) => new(
        beer.Id,
        beer.Name,
        beer.Brewery,
        beer.Style,
        beer.Abv,
        beer.VolumeMl
    );

    public static ParticipantDto ToDto(this Participant participant) => new(
        participant.Id,
        participant.Name
    );

    public static RatingDto ToDto(this Rating rating) => new(
        rating.ParticipantId,
        rating.BeerId,
        rating.Score,
        rating.Comment
    );

    public static DesignRankingDto ToDto(this DesignRanking ranking)
    {
        var list = JsonSerializer.Deserialize<List<string>>(ranking.RankingsJson) ?? new();
        return new DesignRankingDto(ranking.ParticipantId, list);
    }

    private static string ToStatusString(this TastingStatus status) =>
        status switch
        {
            TastingStatus.Pending      => "pending",
            TastingStatus.Active       => "active",
            TastingStatus.DesignRound  => "design_round",
            TastingStatus.Finished     => "finished",
            _                          => "pending"
        };

    public static TastingDto ToDto(this Tasting tasting) => new(
        tasting.Id,
        tasting.Code,
        tasting.Name,
        tasting.HostName,
        tasting.IsBlind,
        tasting.DesignRoundEnabled,
        tasting.BeerList.Select(b => b.ToDto()).ToList(),
        tasting.CurrentBeerIndex,
        tasting.Status.ToStatusString(),
        tasting.Participants.Select(p => p.ToDto()).ToList(),
        tasting.Ratings.Select(r => r.ToDto()).ToList(),
        tasting.DesignRankings.Select(dr => dr.ToDto()).ToList()
    );
}
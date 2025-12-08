using BeerAPI.Models.Dto;
using BeerAPI.Models.Entities;

namespace BeerAPI.Helpers;

public static class TastingMappings
{
    public static BeerDto ToDto(this Beer beer) =>
        new(
            beer.Id,
            beer.Name,
            beer.Brewery,
            beer.Style,
            beer.Abv,
            beer.VolumeMl
        );

    public static ParticipantDto ToDto(this Participant participant) =>
        new(
            participant.Id,
            participant.Name
        );

    public static RatingDto ToDto(this Rating rating) =>
        new(
            rating.ParticipantId,
            rating.BeerId,
            rating.Score,
            rating.Comment
        );

    public static TastingDto ToDto(this Tasting tasting) =>
        new(
            tasting.Id,
            tasting.Code,
            tasting.Name,
            tasting.HostName,
            tasting.IsBlind,
            tasting.BeerList.Select(b => b.ToDto()).ToList(),
            tasting.CurrentBeerIndex,
            tasting.Status.ToString().ToLowerInvariant(), // 'pending', 'active', 'finished'
            tasting.Participants.Select(p => p.ToDto()).ToList(),
            tasting.Ratings.Select(r => r.ToDto()).ToList()
        );
}
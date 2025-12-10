using BeerAPI.Helpers;
using BeerAPI.Models.Dto;
using BeerAPI.Models.Entities;
using BeerTasting.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TastingsController(
    BeerTastingDbContext db,
    ILogger<TastingsController> logger)
    : ControllerBase
{
    // Generate an 8-char tasting code like the mock API
    private static string GenerateCode()
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var random = Random.Shared;
        return new string(Enumerable.Range(0, 8)
            .Select(_ => chars[random.Next(chars.Length)])
            .ToArray());
    }

    /// POST /api/tastings
    /// Creates a new beer tasting session
    [HttpPost]
    public async Task<ActionResult<TastingDto>> CreateTasting(
        [FromBody] CreateTastingRequest request,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.HostName))
        {
            return BadRequest("Name and HostName are required.");
        }

        var code = GenerateCode();

        var tasting = new Tasting
        {
            Name = request.Name,
            HostName = request.HostName,
            IsBlind = request.IsBlind,
            DesignRoundEnabled = request.HasDesignRound,
            Code = code,
            Status = TastingStatus.Pending,
            CurrentBeerIndex = 0
        };

        foreach (var beerDto in request.Beers)
        {
            tasting.BeerList.Add(new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = beerDto.Name,
                Brewery = beerDto.Brewery,
                Style = beerDto.Style,
                Abv = beerDto.Abv,
                VolumeMl = beerDto.VolumeMl,
                IsCatalogBeer = false,
                Tasting = tasting
            });
        }


        db.Tastings.Add(tasting);
        await db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(GetTastingByCode),
            new { code = tasting.Code },
            tasting.ToDto());
    }

    /// GET /api/tastings/{code}
    /// Retrieves a tasting by its code
    [HttpGet("{code}")]
    public async Task<ActionResult<TastingDto>> GetTastingByCode(
        string code,
        CancellationToken ct)
    {
        var normalizedCode = code.ToUpperInvariant();

        var tasting = await db.Tastings
            .Include(t => t.BeerList)
            .Include(t => t.Participants)
            .Include(t => t.Ratings)
            .Include(t => t.DesignRankings)
            .FirstOrDefaultAsync(t => t.Code == normalizedCode, ct);

        if (tasting is null)
            return NotFound("Tasting not found");

        return Ok(tasting.ToDto());
    }

    /// POST /api/tastings/{code}/participants
    /// Joins a tasting as a participant
    [HttpPost("{code}/participants")]
    public async Task<ActionResult<ParticipantDto>> JoinTasting(
        string code,
        [FromBody] JoinTastingRequest request,
        CancellationToken ct)
    {
        var normalizedCode = code.ToUpperInvariant();

        var tasting = await db.Tastings
            .Include(t => t.Participants)
            .FirstOrDefaultAsync(t => t.Code == normalizedCode, ct);

        if (tasting is null)
            return NotFound("Tasting not found");

        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name is required.");

        var participant = new Participant
        {
            Name = request.Name,
            TastingId = tasting.Id
        };

        tasting.Participants.Add(participant);
        await db.SaveChangesAsync(ct);

        return Ok(participant.ToDto());
    }

    /// POST /api/tastings/{tastingId}/start
    /// Starts the tasting session
    [HttpPost("{tastingId}/start")]
    public async Task<IActionResult> StartTasting(
        string tastingId,
        CancellationToken ct)
    {
        var tasting = await db.Tastings
            .FirstOrDefaultAsync(t => t.Id == tastingId, ct);

        if (tasting is null)
            return NotFound("Tasting not found");

        tasting.Status = TastingStatus.Active;
        tasting.CurrentBeerIndex = 0;

        await db.SaveChangesAsync(ct);
        return NoContent();
    }

    /// PUT /api/tastings/{tastingId}/current-beer
    /// Sets the current beer index
    [HttpPut("{tastingId}/current-beer")]
    public async Task<IActionResult> SetCurrentBeer(
        string tastingId,
        [FromBody] SetCurrentBeerRequest request,
        CancellationToken ct)
    {
        var tasting = await db.Tastings
            .Include(t => t.BeerList)
            .FirstOrDefaultAsync(t => t.Id == tastingId, ct);

        if (tasting is null)
            return NotFound("Tasting not found");

        if (request.BeerIndex < 0 || request.BeerIndex >= tasting.BeerList.Count)
            return BadRequest("Invalid beer index.");

        tasting.CurrentBeerIndex = request.BeerIndex;
        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    /// POST /api/tastings/{tastingId}/ratings
    /// Submits a rating for a beer
    [HttpPost("{tastingId}/ratings")]
    public async Task<IActionResult> SubmitRating(
        string tastingId,
        [FromBody] SubmitRatingRequest request,
        CancellationToken ct)
    {
        var tasting = await db.Tastings
            .Include(t => t.Ratings)
            .FirstOrDefaultAsync(t => t.Id == tastingId, ct);

        if (tasting is null)
            return NotFound("Tasting not found");

        if (request.Score is < 1 or > 5)
            return BadRequest("Score must be between 1 and 5.");

        // Ensure participant & beer belong to this tasting
        var participantExists = await db.Participants
            .AnyAsync(p => p.Id == request.ParticipantId && p.TastingId == tastingId, ct);

        var beerExists = await db.Beers
            .AnyAsync(b => b.Id == request.BeerId && b.TastingId == tastingId, ct);

        if (!participantExists || !beerExists)
            return BadRequest("Participant or beer not found in this tasting.");

        // Remove existing rating for this participant/beer combo
        var existing = await db.Ratings
            .FirstOrDefaultAsync(r =>
                    r.TastingId == tastingId &&
                    r.ParticipantId == request.ParticipantId &&
                    r.BeerId == request.BeerId,
                ct);

        if (existing is not null)
        {
            db.Ratings.Remove(existing);
        }

        var rating = new Rating
        {
            TastingId = tastingId,
            ParticipantId = request.ParticipantId,
            BeerId = request.BeerId,
            Score = request.Score,
            Comment = request.Comment
        };

        db.Ratings.Add(rating);
        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    /// POST /api/tastings/{tastingId}/end
    /// Ends the tasting session
    [HttpPost("{tastingId}/end")]
    public async Task<IActionResult> EndTasting(
        string tastingId,
        CancellationToken ct)
    {
        var tasting = await db.Tastings
            .FirstOrDefaultAsync(t => t.Id == tastingId, ct);

        if (tasting is null)
            return NotFound("Tasting not found");

        tasting.Status = TastingStatus.Finished;
        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    [HttpGet("{tastingId}/results")]
    public async Task<ActionResult<TastingResultsDto>> GetResults(
        string tastingId,
        CancellationToken ct)
    {
        var tasting = await db.Tastings
            .Include(t => t.BeerList)
            .Include(t => t.Ratings)
            .Include(t => t.DesignRankings) // NEW
            .FirstOrDefaultAsync(t => t.Id == tastingId, ct);

        if (tasting is null)
            return NotFound("Tasting not found");

        var beers = tasting.BeerList.ToList();

        // ----- 1. Compute tasting-based stats (existing logic) -----
        var beerToRatings = beers.ToDictionary(
            b => b.Id,
            b => tasting.Ratings.Where(r => r.BeerId == b.Id).ToList()
        );

        // ----- 2. Compute design bonus points -----
        var designPoints = beers.ToDictionary(b => b.Id, _ => 0);

        if (tasting.DesignRoundEnabled && tasting.DesignRankings.Count != 0)
        {
            foreach (var ranking in tasting.DesignRankings)
            {
                var orderedIds = System.Text.Json.JsonSerializer
                    .Deserialize<List<string>>(ranking.RankingsJson) ?? new List<string>();

                var validIds = orderedIds.Where(id => beers.Any(b => b.Id == id)).ToList();
                if (validIds.Count == 0) continue;

                var n = validIds.Count;
                for (var i = 0; i < n; i++)
                {
                    var beerId = validIds[i];
                    var points = n - i;
                    designPoints[beerId] += points;
                }
            }
        }

        var maxDesignPoints = designPoints.Values.DefaultIfEmpty(0).Max();
        var beerResults = beers.Select(beer =>
            {
                var ratings = beerToRatings[beer.Id];
                var scores = ratings.Select(r => r.Score).ToList();

                var averageScore = scores.Count > 0
                    ? scores.Average()
                    : 0.0;

                var distribution = Enumerable.Range(1, 5)
                    .Select(score => new DistributionBucketDto(
                        score,
                        ratings.Count(r => r.Score == score)
                    ))
                    .ToList();

                var design = designPoints[beer.Id];

                // Normalize design contribution to 0–1 and add as a small bonus
                var designBonusNormalized = maxDesignPoints > 0
                    ? (double)design / maxDesignPoints
                    : 0.0;

                var combinedScore = Math.Round(averageScore + designBonusNormalized, 2);

                return new BeerResultDto(
                    beer.ToDto(),
                    Math.Round(averageScore, 1),
                    ratings.Count,
                    distribution,
                    design,
                    combinedScore
                );
            })
            .OrderByDescending(b => b.CombinedScore) // combined ranking
            .ToList();

        var results = new TastingResultsDto(beerResults);
        return Ok(results);
    }

    /// POST /api/tastings/{tastingId}/start-design-round
    /// Starts the design ranking round (for blind tastings with design round enabled)
    [HttpPost("{tastingId}/start-design-round")]
    public async Task<IActionResult> StartDesignRound(
        string tastingId,
        CancellationToken ct)
    {
        var tasting = await db.Tastings
            .FirstOrDefaultAsync(t => t.Id == tastingId, ct);

        if (tasting is null)
            return NotFound("Tasting not found");

        if (!tasting.IsBlind)
            return BadRequest("Design round is only available for blind tastings.");

        if (!tasting.DesignRoundEnabled)
            return BadRequest("Design round is not enabled for this tasting.");

        if (tasting.Status != TastingStatus.Active)
            return BadRequest("Design round can only start while the tasting is active.");

        tasting.DesignRoundStarted = true;
        tasting.Status = TastingStatus.DesignRound;
        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    /// POST /api/tastings/{tastingId}/design-rankings
    /// Submits a participant's design ranking (drag-and-drop order)
    [HttpPost("{tastingId}/design-rankings")]
    public async Task<IActionResult> SubmitDesignRanking(
        string tastingId,
        [FromBody] SubmitDesignRankingRequest request,
        CancellationToken ct)
    {
        var tasting = await db.Tastings
            .Include(t => t.BeerList)
            .Include(t => t.DesignRankings)
            .FirstOrDefaultAsync(t => t.Id == tastingId, ct);

        if (tasting is null)
            return NotFound("Tasting not found");

        if (!tasting.DesignRoundEnabled)
            return BadRequest("Design round is not enabled for this tasting.");

        if (!tasting.DesignRoundStarted)
            return BadRequest("Design round has not started yet.");

        // Validate participant belongs to this tasting
        var participant = await db.Participants
            .FirstOrDefaultAsync(p => p.Id == request.ParticipantId && p.TastingId == tastingId, ct);

        if (participant is null)
            return BadRequest("Participant not found in this tasting.");

        // Validate rankings contain only beers from this tasting
        var tastingBeerIds = tasting.BeerList.Select(b => b.Id).ToHashSet();
        if (request.Rankings.Any(id => !tastingBeerIds.Contains(id)))
            return BadRequest("Rankings contain beers that do not belong to this tasting.");

        // Optional: ensure at least 2 beers ranked
        if (request.Rankings.Count < 2)
            return BadRequest("Please rank at least two beers.");

        // Upsert one DesignRanking per participant
        var existing = await db.DesignRankings
            .FirstOrDefaultAsync(dr => dr.TastingId == tastingId && dr.ParticipantId == request.ParticipantId, ct);

        var rankingsJson = System.Text.Json.JsonSerializer.Serialize(request.Rankings);

        if (existing is null)
        {
            var ranking = new DesignRanking
            {
                TastingId = tastingId,
                ParticipantId = request.ParticipantId,
                RankingsJson = rankingsJson
            };

            db.DesignRankings.Add(ranking);
        }
        else
        {
            existing.RankingsJson = rankingsJson;
            db.DesignRankings.Update(existing);
        }

        await db.SaveChangesAsync(ct);
        return NoContent();
    }
}
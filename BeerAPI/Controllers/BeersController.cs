using BeerAPI.Models.Dto;
using BeerTasting.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BeersController(BeerTastingDbContext db) : ControllerBase
{
    /// <summary>
    /// GET /api/beers
    /// Returns the beer catalog
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BeerDto>>> GetBeerCatalog(CancellationToken ct)
    {
        var beers = await db.Beers
            .Where(b => b.IsCatalogBeer)
            .OrderBy(b => b.Name)
            .ToListAsync(ct);

        var result = beers.Select(b => new BeerDto(
            b.Id,
            b.Name,
            b.Brewery,
            b.Style,
            b.Abv,
            b.VolumeMl
        ));

        return Ok(result);
    }
}

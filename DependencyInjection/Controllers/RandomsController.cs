using DependencyInjection.Randomer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjection.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RandomsController : ControllerBase
{
    private readonly IRandomer rnd1;
    private readonly IRandomer rnd2;
    public RandomsController(IRandomer rnd1, IRandomer rnd2)
    {
        this.rnd1 = rnd1;
        this.rnd2 = rnd2;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new RandomInts(rnd1.GetRandomNumber(), rnd2.GetRandomNumber()));
    }
}

class RandomInts
{
    public RandomInts(int r1, int r2)
    {
        rnd1 = r1;
        rnd2 = r2;
    }
    public int rnd1 { get; set; }
    public int rnd2 { get; set; }
}
using Bridge.Application.Dogs.Commands.CreateNewDog;
using Bridge.Application.Dogs.Queries.GetAllDogs;
using Bridge.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bridge.Controllers;

[ApiController]
public class DogsController : ApiContollerBase
{
    private readonly PingMessageOptions _options;

    public DogsController(IOptions<PingMessageOptions> options)
    {
        _options = options.Value;
    }

    [HttpGet]
    [Route("ping")]
    public string Ping()
    {
        return _options.Message;
    }

    [HttpGet]
    [Route("dogs")]
    public async Task<IActionResult> GetDogs(
        [FromQuery] string? attribute,
        [FromQuery] int pageNumber = GetAllDogsDefaultValues.DEFAULT_PAGE_NUMBER,
        [FromQuery] int pageSize = GetAllDogsDefaultValues.DEFAULT_PAGE_SIZE,
        [FromQuery] string order = GetAllDogsDefaultValues.DEFAULT_ORDER)
    {
        var query = new GetAllDogsQuery(
             attribute,
             order,
             pageNumber,
             pageSize);

        var dogs = await Mediator.Send(query);

        return Ok(dogs);
    }

    [HttpPost]
    [Route("dog")]
    public async Task<IActionResult> Create(CreateNewDogCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}

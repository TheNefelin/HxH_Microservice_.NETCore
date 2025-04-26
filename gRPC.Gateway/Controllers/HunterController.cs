using gRPC.HunterService.Protos;
using Microsoft.AspNetCore.Mvc;

namespace gRPC.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HunterController : ControllerBase
{
    private readonly HunterServiceProto.HunterServiceProtoClient _client;

    public HunterController(HunterServiceProto.HunterServiceProtoClient client)
    {
        _client = client;
    }

    // GET: api/Hunter
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HunterResponse>>> GetAll()
    {
        var response = await _client.GetAllHunterAsync(new gRPC.HunterService.Protos.Empty());
        return Ok(response.Hunters);
    }
}

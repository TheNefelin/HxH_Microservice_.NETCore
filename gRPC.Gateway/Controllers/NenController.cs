using gRPC.NenService.Protos;
using Microsoft.AspNetCore.Mvc;

namespace gRPC.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NenController : ControllerBase
{
    private readonly NenServiceProto.NenServiceProtoClient _client;

    public NenController(NenServiceProto.NenServiceProtoClient client)
    {
        _client = client;
    }

    // GET: api/Nen
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NenTypeResponse>>> GetAll()
    {
        var response = await _client.GetAllNenTypesAsync(new gRPC.NenService.Protos.Empty());
        return Ok(response.NenTypes);
    }
}

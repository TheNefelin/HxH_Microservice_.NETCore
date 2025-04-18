using gRPC.HunterNenService;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace gRPC.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HunterNenController : ControllerBase
{
    private readonly HunterNenProto.HunterNenProtoClient _client;

    public HunterNenController(HunterNenProto.HunterNenProtoClient client)
    {
        _client = client;
    }

    // GET: api/HunterNen
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HunterNenResponse>>> GetAll()
    {
        var response = await _client.HunterNenGetAllAsync(new gRPC.HunterNenService.Empty());
        return Ok(response.HunterNens);
    }

    // GET: api/HunterNen/1/2
    [HttpGet("{idHunter}/{idNen}")]
    public async Task<ActionResult<HunterNenResponse>> GetById(int idHunter, int idNen)
    {
        var request = new HunterNenRequest
        {
            IdHunter = idHunter,
            IdNen = idNen
        };

        try
        {
            var response = await _client.HunterNenGetByIdAsync(request);
            return Ok(response);
        }
        catch (RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return NotFound(ex.Status.Detail); // 404 con mensaje
        }
        catch (RpcException ex)
        {
            // Otro error de gRPC
            return StatusCode((int)HttpStatusCode.InternalServerError, ex.Status.Detail);
        }
    }

    // POST: api/HunterNen
    [HttpPost]
    public async Task<ActionResult<GenericResponse>> Insert([FromBody] HunterNenRequest request)
    {
        var response = await _client.HunterNenInsertAsync(request);
        return Ok(response);
    }

    // PUT: api/HunterNen
    [HttpPut]
    public async Task<ActionResult<GenericResponse>> Update([FromBody] HunterNenRequest request)
    {
        var response = await _client.HunterNenUpdateAsync(request);
        return Ok(response);
    }

    // DELETE: api/HunterNen/1/2
    [HttpDelete("{idHunter}/{idNen}")]
    public async Task<ActionResult<GenericResponse>> Delete(int idHunter, int idNen)
    {
        var request = new HunterNenRequest
        {
            IdHunter = idHunter,
            IdNen = idNen
        };

        var response = await _client.HunterNenDeleteAsync(request);
        return Ok(response);
    }
}

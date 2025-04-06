// See https://aka.ms/new-console-template for more information
using gRPC.HunterService.Protos;
using Grpc.Net.Client;

Console.WriteLine("Testing gRPC!");

string server = "https://localhost:7000";
using var channel = GrpcChannel.ForAddress(server);
var client = new HunterServiceProto.HunterServiceProtoClient(channel);

// Insert (CreateHunter)
var createResponse = await client.CreateHunterAsync(new HunterRequest
{
    Name = "Hisoka",
    Age = 28,
    Origin = "Ciudad desconocida"
});
Console.WriteLine($"🟢 CreateHunter: {createResponse.Message}");

// GetAll (GetAllHunter)
var allHunters = await client.GetAllHunterAsync(new Empty());
Console.WriteLine($"📜 All Hunters ({allHunters.Hunters.Count}):");
foreach (var h in allHunters.Hunters)
{
    Console.WriteLine($"- {h.Id}: {h.Name}, {h.Age}, {h.Origin}");
}

// Update (UpdateHunter)
if (allHunters.Hunters.Count > 0)
{
    var hunterWithHighestId = allHunters.Hunters.OrderByDescending(h => h.Id).FirstOrDefault();
    var updateResponse = await client.UpdateHunterAsync(new HunterUpdateRequest
    {
        Id = hunterWithHighestId.Id,
        Name = "Hisoka Morow's",
        Age = hunterWithHighestId.Age,
        Origin = hunterWithHighestId.Origin
    });
    Console.WriteLine($"📝 UpdateHunter: {updateResponse.Message}");
}

// GetById (GetHunterById)
if (allHunters.Hunters.Count > 0)
{
    var lastId = allHunters.Hunters.OrderByDescending(h => h.Id).FirstOrDefault().Id;
    try
    {
        var hunter = await client.GetHunterByIdAsync(new HunterIdRequest { Id = lastId });
        Console.WriteLine($"🔍 GetHunterById({lastId}): {hunter.Name}, {hunter.Age}, {hunter.Origin}");
    }
    catch (Grpc.Core.RpcException ex)
    {
        Console.WriteLine($"❌ Error getting hunter by ID: {ex.Status.Detail}");
    }
}

// Delete (DeleteHunter)
if (allHunters.Hunters.Count > 0)
{
    var lastId = allHunters.Hunters[^1].Id;
    var deleteResponse = await client.DeleteHunterAsync(new HunterIdRequest { Id = lastId });
    Console.WriteLine($"🗑️ DeleteHunter({lastId}): {deleteResponse.Message}");
}

Console.WriteLine("✅ gRPC test finished.");
Console.ReadKey();
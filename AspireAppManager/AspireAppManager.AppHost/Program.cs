var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.gRPC_HunterNenService>("grpc-hunternenservice");

builder.AddProject<Projects.gRPC_HunterService>("grpc-hunterservice");

builder.AddProject<Projects.gRPC_NenService>("grpc-nenservice");

builder.AddProject<Projects.gRPC_Gateway>("grpc-gateway");

builder.Build().Run();

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Anirut_Evacuation_Api>("anirut-evacuation-api");

builder.Build().Run();

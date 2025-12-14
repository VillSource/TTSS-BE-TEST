var builder = DistributedApplication.CreateBuilder(args);

var postgresServer = builder.AddPostgres("postgreSQLServer")
    .WithPgAdmin();

var exampleDatabase = postgresServer.AddDatabase("exampleDB");

builder.AddProject<Projects.Anirut_Evacuation_Api>("anirut-evacuation-api")
    .WaitFor(exampleDatabase)
    .WithReference(exampleDatabase);

builder.Build().Run();

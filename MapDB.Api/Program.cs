using System.Net.Mime;
using System.Text.Json;
using MapDB.Api.Configuration;
using MapDB.Api.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// serialiser allows the string to be human-friendly
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

// Adds MongoDB as a service
// var mongoDBSettings = builder.Configuration.GetSection(nameof(MongoDBConfig)).Get<MongoDBConfig>();
string Host = Environment.GetEnvironmentVariable("MONGO_HOST");
int Port = Int32.Parse(Environment.GetEnvironmentVariable("MONGO_PORT"));
string User = Environment.GetEnvironmentVariable("MONGO_USER");
string Password = Environment.GetEnvironmentVariable("MONGO_PASSWORD");

var mongoDBSettings = new MongoDBConfig(Host, Port, User, Password);

builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
    {
    return new MongoClient(mongoDBSettings.ConnectionString);
    });

builder.Services.AddSingleton<IPinsRepository, MongoDBRepository>();

// Register services
builder.Services.AddControllers().AddMvcOptions(options =>
{
    options.SuppressAsyncSuffixInActionNames = false; // allows async suffix in method calls
})
.AddJsonOptions(options =>{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
}
);

// REST API health checks
builder.Services.AddHealthChecks()
                .AddMongoDb(
                    mongoDBSettings.ConnectionString,
                    name: "mongodb",
                    timeout: TimeSpan.FromSeconds(5),
                    tags: new[] { "ready" });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment()){
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MapDB"));
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthorization();

// checks if server is healthy
app.MapControllers();
app.MapHealthChecks("/health/ready", new HealthCheckOptions{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async(context, report) => // renders health check results message
    { 
        var result = JsonSerializer.Serialize(
            new{
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new{ // array of entries
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception // only returns exception if there is one
                            != null ? entry.Value.Exception.Message : "none", 
                    duration = entry.Value.Duration.ToString()
                })
            }
        );
        context.Response.ContentType = MediaTypeNames.Application.Json; // renders as Json string in Postman
        await context.Response.WriteAsync(result);
    } 
});

// checks if server is live
app.MapHealthChecks("/health/live", new HealthCheckOptions{
    Predicate = (_) => false // excludes every health checking, including MongoDB
});


app.Use(async (context, next) => // context = any http request
{
    context.Response.Headers["Access-Control-Allow-Origin"] = "https://www.thegoodcarbonfarm.com";
    context.Response.Headers["Access-Control-Allow-Headers"] = "Origin, X-Requested-With, Content-Type, Accept";
    
    if (context.Request.Method == HttpMethods.Options){
        context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
        context.Response.StatusCode = (int)HttpStatusCode.OK;
        return;
    }
    
    await next(); // moves on to the next task
});

app.Run();


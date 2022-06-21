using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccessControl.Api;

public static class Guard
{
    [FunctionName("Guard")]
    public static async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "patch", Route = null)]
        HttpRequest req, ILogger log)
    {
        log.LogInformation("Function started");
        
        var cards = new CardRepository(
            Environment.GetEnvironmentVariable("CosmosDbConnectionString", EnvironmentVariableTarget.Process));
        log.LogInformation("Repository created");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string id = data?.id;
        byte[] image = data?.image;
        log.LogInformation("Request data retrieved");

        if (id == null)
        {
            return new BadRequestObjectResult("Please pass an id in the request body");
        }
        if (image == null)
        {
            return new BadRequestObjectResult("Please pass an image in the request body");
        }
        log.LogInformation("Request validated");

        var card = cards.FirstOrCreate(id);
        log.LogInformation($"Card retrieved with id {id}");
        
        cards.AddAccessEntry(id, image);
        log.LogInformation("Access entry added");

        return new OkObjectResult(card.CanCardAccess);
    }
}
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using restapi.Models;
using System.Collections.Generic;
using System.Linq;

namespace restapi
{
    public static class GameListApi
    {
        static List<GameItem> gameItemsList = new();

        // Get all
        [FunctionName("GetGameItemList")]
        public static async Task<IActionResult> GetGameItemList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gameitems")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting all Games");

            return new OkObjectResult(gameItemsList);
        }

        // Get byId
        [FunctionName("GetGameItemById")]
        public static async Task<IActionResult> GetGameItemById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "gameitems/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Getting a specific game by Id");

            var gameItem = gameItemsList.FirstOrDefault(q => q.Id == id);
            if (gameItem == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(gameItem);
        }

        // Post - Create a Game Item
        [FunctionName("GreateGameItem")]
        public static async Task<IActionResult> GreateGameItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "gameitems")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating an Game item");

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<CreateGameItem>(requestData);

            var item = new GameItem
            {
                ItemName = data.ItemName
            };

            gameItemsList.Add(item);

            return new OkObjectResult(item);
        }

        // Update
        [FunctionName("PutGameItem")]
        public static async Task<IActionResult> GetGame(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "gameitems/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Updating an game item");

            var item = gameItemsList.FirstOrDefault(q => q.Id == id);
            if (item == null) 
            { 
                return new NotFoundResult(); 
            }

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UpdateGameItem>(requestData);

            item.Played = data.Played;

            return new OkObjectResult(item);
        }

        // Delete
        [FunctionName("DeleteGameItem")]
        public static async Task<IActionResult> DeleteGameItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "gameitems/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Deleting an game from the list ");

            var item = gameItemsList.FirstOrDefault(q => q.Id == id);
            if (item == null)
            {
                return new NotFoundResult();
            }

            gameItemsList.Remove(item);

            return new OkResult();
        }

    }
}

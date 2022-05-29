using FunctionAppCRUD.Data;
using FunctionAppCRUD.Data.Entities;
using FunctionAppCRUD.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionAppCRUD
{
    public class ConstructionAdd
    {
        private readonly IConstructionDataStore _constructionDataStore;
        private readonly ILogger _logger;
        private const string _baseRoute = "constructions";

        public ConstructionAdd(IConstructionDataStore constructionDataStore, ILoggerFactory loggerFactory)
        {
            _constructionDataStore = constructionDataStore;
            _logger = loggerFactory.CreateLogger<ConstructionAdd>();
        }

        [Function(nameof(ConstructionAdd))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = _baseRoute)] HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(ConstructionAdd)} processed a request.");

            var construction =
                await req.Body.DeserializeAsync<Construction>();

            if (construction is null)
                return new BadRequestResult();

            await _constructionDataStore.AddAsync(construction);
            return new CreatedResult($"/{_baseRoute}/{construction.Id}", construction);
        }
    }
}

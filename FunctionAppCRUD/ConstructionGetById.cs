//using FunctionAppCRUD.Data;
//using FunctionAppCRUD.Data.Entities;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.Logging;

//namespace FunctionAppCRUD
//{
//    public class ConstructionGetById
//    {
//        private readonly IConstructionDataStore constructionDataStore;

//        public ConstructionGetById(
//            IConstructionDataStore ConstructionEntityDataStore)
//        {
//            constructionDataStore = ConstructionEntityDataStore;
//        }

//        [Function(nameof(ConstructionGetById))]
//        public async Task<IActionResult> Run(
//            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
//            ILogger log)
//        {
//            log.LogInformation($"{nameof(ConstructionGetById)} function processed a request.");

//            string id = req.Query["id"];

//            if (string.IsNullOrWhiteSpace(id))
//            {
//                throw new ArgumentNullException(nameof(id));
//            }

//            var construction = await constructionDataStore.GetByIdAsync(id);
//            if (construction == null) return new NotFoundResult();
//            return new OkObjectResult(construction);
//        }
//    }
//}

using FunctionAppCRUD.Core.Entity;
using FunctionAppCRUD.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAzure.Storage;

var entityDataStoreOptions =
    new EntityDataStoreOptions();

#if DEBUG
        entityDataStoreOptions.CloudTableClientPrimary = CloudStorageAccount.DevelopmentStorageAccount.CreateCloudTableClient();
#else
        entityDataStoreOptions.CloudTableClientPrimary = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("PrimaryConnectionString"))
            .CreateCloudTableClient();
#endif

var HostBuilder = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((hostBuilder, config) =>
    {

    })
    .ConfigureServices(s =>
    {
        s.AddSingleton(entityDataStoreOptions);
        s.AddTransient<IConstructionDataStore, ConstructionDataStore>();
    });

await HostBuilder.Build().RunAsync();
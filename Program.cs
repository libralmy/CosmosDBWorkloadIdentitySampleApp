using Azure.Identity;
using Azure.ResourceManager.CosmosDB;
using Azure.ResourceManager.CosmosDB.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Management.CosmosDB;
using System.Configuration;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace CosmosDBMgmtTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var subscriptionId = ConfigurationManager.AppSettings["SubscriptionId"];
            var resourceGroupName = ConfigurationManager.AppSettings["ResourceGroupName"]; 
            var accountName = ConfigurationManager.AppSettings["AccountName"]; 
            var databaseName = ConfigurationManager.AppSettings["DatabaseName"]; 
            var containerName = ConfigurationManager.AppSettings["ContainerName"]; 
            var userAssignedClientId = ConfigurationManager.AppSettings["UserAssignedClientId"];

            /*Docker: get env vars*/
            subscriptionId = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CDBAKS_SUB")) ? subscriptionId : Environment.GetEnvironmentVariable("CDBAKS_SUB");
            resourceGroupName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CDBAKS_RG")) ? resourceGroupName : Environment.GetEnvironmentVariable("CDBAKS_RG");
            accountName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CDBAKS_DBACCT")) ? accountName : Environment.GetEnvironmentVariable("CDBAKS_DBACCT");
            databaseName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CDBAKS_DB")) ? databaseName : Environment.GetEnvironmentVariable("CDBAKS_DB");
            containerName = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CDBAKS_CONT")) ? containerName : Environment.GetEnvironmentVariable("CDBAKS_CONT");
            userAssignedClientId = String.IsNullOrEmpty(Environment.GetEnvironmentVariable("CDBAKS_UCLID")) ? userAssignedClientId : Environment.GetEnvironmentVariable("CDBAKS_UCLID");

            // create the management clientSS
            var tokenCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = userAssignedClientId });
            var managementClient = new Azure.ResourceManager.CosmosDB.CosmosDBManagementClient(subscriptionId, tokenCredential);

            // create a new database 
            var createDatabaseOperation = await managementClient.SqlResources.StartCreateUpdateSqlDatabaseAsync(resourceGroupName, accountName, databaseName,
            new SqlDatabaseCreateUpdateParameters(new SqlDatabaseResource(databaseName), new CreateUpdateOptions()));
            await createDatabaseOperation.WaitForCompletionAsync();

            // create a new container
            var createContainerOperation = await managementClient.SqlResources.StartCreateUpdateSqlContainerAsync(resourceGroupName, accountName, databaseName, containerName,
            new SqlContainerCreateUpdateParameters(new SqlContainerResource(containerName), new CreateUpdateOptions()));
            await createContainerOperation.WaitForCompletionAsync();

        
        }
    }
}

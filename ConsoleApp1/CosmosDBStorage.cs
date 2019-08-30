using System;
using System.Linq;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

namespace ConnectToDBApp
{
    class CosmosDBStorage
    {
        static string DatabaseName = "maindb";
        static string CollectionName = "employee";
        static DocumentClient dc;

        static string endpoint = "https://<cosmosdburl>.documents.azure.com:443/";
        static string key = " <need Cosmos DB key> ";

        static void CosmosMain(string[] args)
        {
            dc = new DocumentClient(new Uri(endpoint), key);

            InsertOp("john", "doe");
            InsertOp("tony", "soprano");
            InsertOp("richard", "smith");
            QueryOp();

            Console.WriteLine("\n\n");
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }

        static void InsertOp(string first, string last)
        {
            CosmosDBEmployeeEntity employeenumber1 = new CosmosDBEmployeeEntity();
            employeenumber1.FirstName = first;
            employeenumber1.LastName = last;

            var result = dc.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), 
                employeenumber1
            ).GetAwaiter().GetResult();
        }

        static void QueryOp()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            IQueryable<CosmosDBEmployeeEntity> query = dc.CreateDocumentQuery<CosmosDBEmployeeEntity>(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), queryOptions)
                .Where(e => e.LastName == "soprano");

            Console.WriteLine("Names of all of the staff:");
            Console.WriteLine("==========================");
            foreach (CosmosDBEmployeeEntity employee in query)
            {
                Console.WriteLine(employee);
            }
        }
    }

    public class CosmosDBEmployeeEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

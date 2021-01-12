using Microsoft.Azure.Cosmos.Table;
using System;

namespace ConnectToDBApp
{
    class TableStorage
    {
        static CloudStorageAccount storageAccount;
        static CloudTableClient tableClient;
        static CloudTable employees;

        static void TableMain(string[] args)
        {
            storageAccount = CloudStorageAccount.Parse(
        "DefaultEndpointsProtocol=https;AccountName=<storage account name>;AccountKey=<storage account key>;EndpointSuffix=core.windows.net");
            tableClient = storageAccount.CreateCloudTableClient();
            employees = tableClient.GetTableReference("Employees");

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
            EmployeeEntityTable employeenumber1 = new EmployeeEntityTable(first, last);

            TableOperation insertop = TableOperation.Insert(employeenumber1);
            employees.Execute(insertop);
        }

        static void QueryOp()
        {
            TableQuery<EmployeeEntityTable> query = new TableQuery<EmployeeEntityTable>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "staff"));

            Console.WriteLine("Names of all of the staff:");
            Console.WriteLine("==========================");
            foreach (EmployeeEntityTable employee in employees.ExecuteQuery(query))
            {
                Console.WriteLine(employee.RowKey);
            }
        }
    }

    public class EmployeeEntityTable : TableEntity
    {
        public EmployeeEntityTable(string firstname, string lastname)
        {
            this.PartitionKey = "staff";
            this.RowKey = firstname + " " + lastname;
        }
        public EmployeeEntityTable() { }
          
    }
}

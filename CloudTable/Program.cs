using Microsoft.Azure;
using Microsoft.Azure.CosmosDB.Table;
using Microsoft.Azure.Storage;
using System;
using System.Collections.Generic;

namespace TableStorage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudTableClient cloudTableClient = new CloudTableClient(cloudStorageAccount.TableStorageUri, cloudStorageAccount.Credentials);

            CloudTable table = cloudTableClient.GetTableReference("people");

            table.CreateIfNotExists();

            //Insert(table);
            //BulkInsert(table);

            //GetAllPerson(table);
            //GetAllPersonByPartitionKey(table);

            //GetSingle(table);

            ///Update(table);

            //Delete(table);
            GetAllPerson(table);
            
            //DeleteTable(table);
        }

        public static void BulkInsert(CloudTable table)
        {
            Person person1 = new Person("frisco", "anshuman")
            {
                City = "Frisco",
                Name = "Anshuman",
                PersonId = 2,
                SNN = "sadasd"
            };

            Person person2 = new Person("frisco", "harsha")
            {
                City = "Plano",
                Name = "Harsha",
                PersonId = 2,
                SNN = "asa"
            };

            TableBatchOperation tableOperations = new TableBatchOperation();
            tableOperations.Insert(person1);
            tableOperations.Insert(person2);

            table.ExecuteBatch(tableOperations);
        }

        public static void Insert(CloudTable table)
        {
            Person person = new Person("plano", "vaishnav");

            person.City = "plano";
            person.Name = "vaishnav";
            person.PersonId = 1;
            person.SNN = "353254668";

            TableOperation insertOperation = TableOperation.Insert(person);
            table.Execute(insertOperation);
        }

        public static void GetAllPerson(CloudTable table)
        {
            TableQuery<Person> personQuery = new TableQuery<Person>();

            var persons = table.ExecuteQuery(personQuery);

            foreach (Person item in persons)
            {
                Console.WriteLine("===========================================================");

                Console.WriteLine("Person ID : {0}", item.PersonId);
                Console.WriteLine("Person's Name : {0}", item.Name);
                Console.WriteLine("Person's City : {0}", item.City);
                Console.WriteLine("Person's SNN : {0}", item.SNN);

                Console.WriteLine("===========================================================");
            }

            Console.ReadLine();
        }

        public static void GetAllPersonByPartitionKey(CloudTable table)
        {
            string where = TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "frisco"), 
                                                     TableOperators.And, 
                                                     TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, "i"));
            TableQuery<Person> personQuery = new TableQuery<Person>().Where(where);

            var persons = table.ExecuteQuery(personQuery);

            foreach (Person item in persons)
            {
                Console.WriteLine("===========================================================");

                Console.WriteLine("Person ID : {0}", item.PersonId);
                Console.WriteLine("Person's Name : {0}", item.Name);
                Console.WriteLine("Person's City : {0}", item.City);
                Console.WriteLine("Person's SNN : {0}", item.SNN);

                Console.WriteLine("===========================================================");
            }

            Console.ReadLine();
        }

        public static void GetSingle(CloudTable table)
        {
            TableOperation tableOperation = TableOperation.Retrieve<Person>("plano", "vaishnav", new List<string>()
            {
                "City",
                "PersonId",
                "Name"
            });

            TableResult result = table.Execute(tableOperation);

            Person item = (Person)result.Result;

            Console.WriteLine("===========================================================");

            Console.WriteLine("Person ID : {0}", item.PersonId);
            Console.WriteLine("Person's Name : {0}", item.Name);
            Console.WriteLine("Person's City : {0}", item.City);
            Console.WriteLine("Person's SNN : {0}", item.SNN);

            Console.WriteLine("===========================================================");

            Console.ReadLine();
        }

        public static void Update(CloudTable table)
        {
            TableOperation tableOperation = TableOperation.Retrieve<Person>("frisco", "harsha");

            TableResult result = table.Execute(tableOperation);

            if(result.Result != null)
            {
                Person p = (Person)result.Result;

                p.City = "Frisco";

                TableOperation updateOperation = TableOperation.Replace(p);

                table.Execute(updateOperation);
            }

            GetAllPersonByPartitionKey(table);
        }

        public static void Delete(CloudTable table)
        {
            TableOperation tableOperation = TableOperation.Retrieve<Person>("plano", "vaishnav");

            TableResult result = table.Execute(tableOperation);

            if(result != null)
            {
                TableOperation deleteOperation = TableOperation.Delete((Person)result.Result);

                table.Execute(deleteOperation);

                Console.WriteLine("Deleted");
            }
        }

        public static void DeleteTable(CloudTable table)
        {
            table.DeleteIfExists();
        }
    }
}

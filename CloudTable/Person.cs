using Microsoft.Azure.CosmosDB.Table;

namespace TableStorage
{
    /// <summary>
    /// Class must inherit from Microsoft.Azure.CosmosDB.Table.TableEntity in order to Map entity to a custom class.
    /// Custom class must have a default parameterless constructure.
    /// It should define the partition key and row key in order to faster traversal of the data with in the Entity table.
    /// C# properties you want to have in entity schema in db much be declared as public and with both getter and setters.
    /// </summary>
    public class Person : TableEntity
    {
        public int PersonId { get; set; }
        public string Name { get; set; }

        public string City { get; set; }

        public string SNN { get; set; }

        public Person() { }

        public Person(string city, string name)
        {
            this.PartitionKey = city;
            this.RowKey = name;
        }
    }
}

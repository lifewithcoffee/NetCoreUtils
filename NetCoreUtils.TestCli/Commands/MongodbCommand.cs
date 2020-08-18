using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreCmd.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using NetCoreUtils.Database.MongoDb;
using NetCoreUtils.TestCli.MongoDbDemo;

namespace NetCoreUtils.TestCli.Commands
{
    [Alias("mongo")]
    public class MongodbCommand : MongodbCommandBase { }

    public class Mongo2Command
    {
        IMongoDbConnection mongo;

        // note: the following members use different collection names, see the constructor for more details
        IMongoCollection<TestDoc> collection;
        IMongoDocReader<Student> reader;

        public Mongo2Command(IMongoDbConnection connection, IMongoDocReader<Student> reader)
        {
            this.reader = reader;
            this.reader.CollectionName = "rl_test_colle2";

            this.mongo = connection;
            this.collection = connection.MongoDatabase.GetCollection<TestDoc>("TestDocs");
        }

        public async Task AddDoc()
        {
            Console.WriteLine("AddDoc() start writing ...");
            await collection.InsertOneAsync(
                new TestDoc
                {
                    StrField = "some test",
                    IntField = 134,
                });
            Console.WriteLine("AddDoc() end writing.");
        }

        public void ReadDoc()
        {
            var doc = collection.Find("5f3bbd65f63d7d5175a805d8");
            Console.WriteLine(doc.ToJson());
        }

        public void ReadAllDocs()
        {
            var docs = reader.Find(d => true);
            foreach (var doc in docs)
            {
                Console.WriteLine(doc.ToJson());
            }
        }
    }

    public class Score
    {
        public string type;
        public double score;
    }

    public class Student : MongoDoc
    {
        public int student_id;
        public List<Score> scores;
        public int class_id;
    }

    public class TestDoc : MongoDoc
    {
        public string StrField { get; set; }
        public int IntField { get; set; }
    }
}

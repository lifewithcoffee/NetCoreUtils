using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreCmd.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using NetCoreUtils.Database.MongoDb;
using TestApp.Cli.Database.MongoDbDemo;

namespace TestApp.Cli.Database.Commands
{
    [Alias("mongo")]
    public class MongodbCommand : MongodbCommandBase { }

    public class Mongo2Command
    {

        // note: the following members use different collection names, see the constructor for more details
        IMongoCollection<TestDoc> collection;

        public Mongo2Command(IMongoDbConnection connection)
        {
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
    }

    public class Mongo3Command
    {

        IMongoDocReader<Student> reader;
        IMongoDocWriter<Student> writer;

        public Mongo3Command(IMongoDocReader<Student> reader, IMongoDocWriter<Student> writer)
        {
            this.reader = reader;
            this.writer = writer;
            Console.WriteLine($"readers collection name = {reader.CollectionName}");
        }

        public async Task Add()
        {
            await writer.InsertOneAsync(new Student
            {
                student_id = 3432,
                class_id = 12,
                scores = new List<Score> {
                    new Score { type = "English", score = 89 },
                    new Score{ type = "Math", score = 91 },
                }
            });
        }

        public void ReadAll()
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

    //[Collection("my_test_collection_name")]
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

using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TestApp.Cli.Database.MongoDbDemo
{
    public class MongodbCommandBase
    {
        const string localDbName = "rltestdb";
        MongoClient _dbClient = new MongoClient("mongodb://localhost:27017");

        public void ListDB()
        {
            Console.WriteLine("The list of databases on this server is: ");
            _dbClient.ListDatabases().ToList().ForEach(db => Console.WriteLine(db));
        }

        public async Task TransactionDemo()
        {
            var collec = _dbClient.GetDatabase(localDbName).GetCollection<BsonDocument>("rl_test_collec");
            //var filter = Builders<BsonDocument>.Filter.Eq(x => x.Name, "b");
            var filter = Builders<BsonDocument>.Filter.Empty;

            //var docs = await collec.FindAsync(new BsonDocument());
            var docs = await collec.FindAsync(filter);

            using (var session = await _dbClient.StartSessionAsync())
            {

            }
        }

        public async Task DropDB()
        {
            await _dbClient.DropDatabaseAsync(localDbName);
        }

        public void DropCollection(string collectionName)
        {
            var db = _dbClient.GetDatabase(localDbName);
            db.DropCollection(collectionName);
        }

        public void CreateCollection(string collectionName)
        {
            var db = _dbClient.GetDatabase(localDbName);
            db.CreateCollection(collectionName);
        }

        public void CreateDoc(string collectionName)
        {
            _dbClient.GetDatabase(localDbName)
                     .GetCollection<BsonDocument>(collectionName)      // [notes] if the collection doesn't exist, the system will create it automatically
                     .InsertOne(new BsonDocument {
                        { "student_id", 10000 },
                        {
                            "scores",
                            new BsonArray {
                                new BsonDocument { { "type", "exam" }, { "score", 88.12334193287023 } },
                                new BsonDocument { { "type", "quiz" }, { "score", 74.92381029342834 } },
                                new BsonDocument { { "type", "homework" }, { "score", 89.97929384290324 } },
                                new BsonDocument { { "type", "homework" }, { "score", 82.12931030513218 } }
                            }
                        },
                        { "class_id", 480 }
                     });
        }

        // remember to execute CreateDoc() several times to generate data first
        public void UpdateDoc(string collectionName, int classId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("student_id", 10000) & Builders<BsonDocument>.Filter.Eq("scores.type", "quiz");   // [notes] multiple condition applied
            var update = Builders<BsonDocument>.Update.Set("class_id", classId);

            _dbClient.GetDatabase(localDbName)
                     .GetCollection<BsonDocument>(collectionName)
                     .UpdateMany(filter, update);        // [notes] if use UpdateOne(), only the first doc will be updated
        }

        public void DeleteDoc(string collectionName)
        {
            // [notes] filter by matching elements in an array
            var deleteLowExamFilter = Builders<BsonDocument>.Filter.ElemMatch<BsonValue>("scores",
                 new BsonDocument { { "type", "exam" }, {"score", new BsonDocument { { "$gt", 60 }}}
            });

            _dbClient.GetDatabase(localDbName)
                     .GetCollection<BsonDocument>(collectionName)
                     .DeleteOne(deleteLowExamFilter);        // [notes] use DeleteMany() to delete all
        }
    }

}

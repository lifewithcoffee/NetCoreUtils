# NetCoreUtils.Database.MongoDb

## Document Class

- Document class should inherit from `MongoDoc`

  ``` csharp
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
  ```

## Document Collection Name

When inject an `IRepositoryRead<TDoc>` or `IRepositoryWrite<TDoc>`:

- The default collection name is `[lower doc name]_collection`

  For example, the above document `Student` will use `student_collection` as its default
  collection name.

- A collection name can be specified explicitly:

  ``` csharp
  public Mongo2Command(IRepositoryRead<Student> reader)
  {
    this.reader = reader;
    this.reader.CollectionName = "rl_test_colle2";
  }
  ```

- A collection name can be specified via a `[Collection]` attribute:
  
  ``` csharp
  [Collection("my_test_collection_name")]
  public class Student : MongoDoc { }
  ```

## Start local MongoDB server

Start local mongodb instance:
> D:\Program Files\MongoDB\Server\4.2\bin\mongod.exe

View data in Compass:
> D:\Program Files\MongoDB\mongodb-compass\MongoDBCompass.exe

# Release Notes

## 1.3.0-working

- Upgrade to .net9
- Upgrade MongoDB.Driver to v3.4.2

## 1.2.0

- Remove MongoDoc.CreateAt since it can be get from ObjectId
- Rename MongoDoc.Id to MongoDoc._id to keep consist with MongoDB's naming
- Add `IMongoDocWriter<TDoc>.Delete(id)` and `IMongoDocWriter<TDoc>.DeleteAsync(id)`

## 1.1.0

- Add IMongoDbConnection.UpdateSetting() method
- Add collection attribute and default collection name
  > if not specified by attribute [collection("a_custom_collection_name")], the default collection name will be `<class_name>_collection`

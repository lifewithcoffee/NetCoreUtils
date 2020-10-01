# NetCoreUtils.Database.MongoDb

## Usage

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

- When inject an `IRepositoryRead<TDoc>` or `IRepositoryWrite<TDoc>`, their
  `CollectionName` must be explicitly assigned.

  ``` csharp
  public Mongo2Command(IRepositoryRead<Student> reader)
  {
    this.reader = reader;
    this.reader.CollectionName = "rl_test_colle2";
  }
  ```

## Start local MongoDB server

Start local mongodb instance:
> D:\Program Files\MongoDB\Server\4.2\bin\mongod.exe

View data in Compass:
> D:\Program Files\MongoDB\mongodb-compass\MongoDBCompass.exe

## Release Notes

## 1.2.0

- Remove MongoDoc.CreateAt since it can be get from ObjectId
- Rename MongoDoc.Id to MongoDoc._id to keep consist with MongoDB's naming
- Add `IMongoDocWriter<TDoc>.Delete(id)` and `IMongoDocWriter<TDoc>.DeleteAsync(id)`

## 1.1.0

- Add IMongoDbConnection.UpdateSetting() method
- Add collection attribute and default collection name
  > if not specified by attribute [collection("a_custom_collection_name")], the default collection name will be `<class_name>_collection`

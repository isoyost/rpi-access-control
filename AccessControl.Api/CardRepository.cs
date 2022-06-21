using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace AccessControl.Api;

public class CardRepository
{
    private readonly IMongoCollection<BsonDocument> _collection;

    public CardRepository(string connectionString)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("accessControl");
        _collection = database.GetCollection<BsonDocument>("cards");
    }

    public Card FirstOrCreate(string id)
    {
        var filterForSentId = Builders<BsonDocument>.Filter.Eq("_id", id);
        var cardDocument = _collection.Find(filterForSentId).FirstOrDefault();
        
        // If card does not exist create it
        if (cardDocument != null) return BsonSerializer.Deserialize<Card>(cardDocument);
        var card = new Card(id);
        _collection.InsertOne(card.ToBsonDocument());

        return card;
    }
    
    public async void AddAccessEntry(string id, byte[] image)
    {
        var filterForSentId = Builders<BsonDocument>.Filter.Eq("_id", id);
        var update = Builders<BsonDocument>.Update.Push("access_tries", new AccessAttempt(image).ToBsonDocument());
        await _collection.UpdateOneAsync(filterForSentId, update);
    }
}
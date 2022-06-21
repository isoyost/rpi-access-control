using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace AccessControl.Api;

public class Card
{
    public Card(string id, string name = "Unidentified Card", bool canCardAccess = false)
    {
        Id = id;
        CanCardAccess = canCardAccess;
        AccessTries = new List<AccessAttempt>();
        Name = name;
    }
    
    [BsonId]
    [JsonProperty("id")]
    public string Id { get; }
    [JsonProperty("can_card_access")]
    [BsonElement("can_card_access")]
    public bool CanCardAccess { get; set; }
    [JsonProperty("access_tries")]
    [BsonElement("access_tries")]
    public List<AccessAttempt> AccessTries { get; }
    [JsonProperty("name")]
    [BsonElement("name")]
    public string Name { get; set; }
}

public class AccessAttempt
{
    public AccessAttempt(byte[] image)
    {
        Timestamp = DateTime.UtcNow;
        Image = image;
    }
    
    [JsonProperty("timestamp")]
    [BsonElement("timestamp")]
    public DateTime Timestamp { get; }
    [JsonProperty("image")]
    [BsonElement("image")]
    public byte[] Image { get; }
}
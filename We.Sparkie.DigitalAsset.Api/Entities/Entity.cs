using System;
using MongoDB.Bson.Serialization.Attributes;

namespace We.Sparkie.DigitalAsset.Api.Entities
{
    public abstract class Entity
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}
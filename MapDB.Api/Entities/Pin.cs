using Microsoft.AspNetCore.Components.Routing;
using MongoDB.Bson.Serialization.Attributes;

namespace MapDB.Api.Entities{
    public class Pin{ // record class has better support for immutable data?

        [BsonId]
        public Guid ID { get; set; } // guid: unique ID
        public string Name { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
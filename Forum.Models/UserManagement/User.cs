using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Forum.Models.UserManagement
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

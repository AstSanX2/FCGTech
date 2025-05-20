using MongoDB.Bson;

namespace FCG.Domain.Entities
{
    public class BaseEntity
    {
        public ObjectId _id { get; set; }
    }
}

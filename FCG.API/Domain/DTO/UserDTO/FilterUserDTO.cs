using FCG.API.Domain.DTO.Bases;
using FCG.Domain.Entities;
using MongoDB.Bson;
using System.Linq.Expressions;

namespace FCG.API.Domain.DTO.UserDTO
{
    public class FilterUserDTO : BaseFilterDTO<User>
    {
        public ObjectId _id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public override Expression<Func<User, bool>> GetFilterExpression()
        {
            return x => x._id == _id;
        }
    }
}

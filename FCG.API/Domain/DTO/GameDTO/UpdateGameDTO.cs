using FCG.API.Domain.DTO.Bases;
using FCG.Domain.Entities;
using MongoDB.Driver;

namespace FCG.API.Domain.DTO.GameDTO
{
    public class UpdateGameDTO : BaseUpdateDTO<Game>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public decimal? Price { get; set; }

        public override UpdateDefinition<Game> GetUpdateDefinition()
        {
            var update = Builders<Game>.Update;
            var updates = new List<UpdateDefinition<Game>>();

            if (!string.IsNullOrWhiteSpace(Name))
                updates.Add(update.Set(x => x.Name, Name));

            if (!string.IsNullOrWhiteSpace(Description))
                updates.Add(update.Set(x => x.Description, Description));

            if (!string.IsNullOrWhiteSpace(Category))
                updates.Add(update.Set(x => x.Category, Category));

            if (ReleaseDate.HasValue)
                updates.Add(update.Set(x => x.ReleaseDate, ReleaseDate.Value));

            if (LastUpdateDate.HasValue)
                updates.Add(update.Set(x => x.LastUpdateDate, LastUpdateDate.Value));

            if (Price.HasValue && Price < 0)
                updates.Add(update.Set(x => x.Price, Price.Value));

            return update.Combine(updates);
        }
    }
}

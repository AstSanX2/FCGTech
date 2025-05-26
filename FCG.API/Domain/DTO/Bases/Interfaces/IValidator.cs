using FCG.API.Domain.Models.Validation;

namespace FCG.API.Domain.DTO.Bases.Interfaces
{
    public interface IValidator
    {
        ValidationResultModel Validate();
    }
}

using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.TolovUsullari;

namespace SmartMarket.Service.Interfaces.TolovUsuli;

public interface ITolovUsuliService
{
    Task<TolovUsuliForResultDto> RetrieveByIdAsync(long id);//
    Task<TolovUsuliForResultDto> CreateAsync(TolovUsuliForCreationDto dto);//
    Task<IEnumerable<TolovUsuliForResultDto>> RetrieveAllAsync(PaginationParams @params);//
    Task<HisobotForResultDto> GetNaqtTolovHisoboti(long kassaId, DateTime startDate, DateTime endDate);
}

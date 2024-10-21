using SmartMarket.Domin.Configurations;
using SmartMarket.Service.DTOs.Users.Payments;

namespace SmartMarket.Service.Interfaces.Users;

public interface IWorkersPaymentService
{
    Task<bool> RemoveAsync(long id);//
    Task<WorkersPaymentForResultDto> RetrieveByIdAsync(long id);//
    Task<WorkersPaymentForResultDto> CreateAsync(WorkersPaymentForCreationDto dto);//
    Task<WorkersPaymentForResultDto> ModifyAsync(WorkersPaymentForUpdateDto dto);//
    Task<IEnumerable<WorkersPaymentForResultDto>> RetrieveAllAsync(PaginationParams @params);//
}

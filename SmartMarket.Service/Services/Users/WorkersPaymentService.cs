using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Users;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.Commons.Helpers;
using SmartMarket.Service.DTOs.Users;
using SmartMarket.Service.DTOs.Users.Payments;
using SmartMarket.Service.Interfaces.Users;

namespace SmartMarket.Service.Services.Users;

public class WorkersPaymentService : IWorkersPaymentService
{
    private readonly IRepository<WorkersPayment> _paymentRepository;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public WorkersPaymentService(IRepository<WorkersPayment> paymentRepository, IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _paymentRepository = paymentRepository;
        _userService = userService;
    }

    public async Task<WorkersPaymentForResultDto> CreateAsync(WorkersPaymentForCreationDto dto)
    {
        var mapped = _mapper.Map<WorkersPayment>(dto);
        if (dto.OlganPuli is not null)
        {
            mapped.QolganPuli = dto.Oylik - dto.OlganPuli;
        }
        mapped.CreatedAt = DateTime.UtcNow;

        await _userService.RetrieveByIdAsync(dto.UserId);
        
        await _paymentRepository.InsertAsync(mapped);

        return _mapper.Map<WorkersPaymentForResultDto>(mapped);
    }

    public async Task<WorkersPaymentForResultDto> ModifyAsync(WorkersPaymentForUpdateDto dto)
    {
        var user = await _paymentRepository.SelectAll()
             .Where(u => u.Id == dto.Id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Oylik topilmadi.");

        await _userService.RetrieveByIdAsync(dto.UserId);

        var mapped = _mapper.Map(dto, user);
        if (dto.OlganPuli is not null)
        {
            mapped.QolganPuli = dto.Oylik - dto.OlganPuli;
        }
        mapped.UpdatedAt = DateTime.UtcNow;

        await _paymentRepository.UpdateAsync(mapped);

        return _mapper.Map<WorkersPaymentForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var user = await _paymentRepository.SelectAll()
              .Where(u => u.Id == id)
              .AsNoTracking()
              .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Oylik topilmadi.");

        await _paymentRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<WorkersPaymentForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = await _paymentRepository.SelectAll()
             .AsNoTracking()
             .ToPagedList(@params)
             .ToListAsync();

        return _mapper.Map<IEnumerable<WorkersPaymentForResultDto>>(users);
    }

    public async Task<WorkersPaymentForResultDto> RetrieveByIdAsync(long id)
    {
        var user = await _paymentRepository.SelectAll()
             .Where(u => u.Id == id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "Oylik topilmadi.");

        return _mapper.Map<WorkersPaymentForResultDto>(user);
    }
}

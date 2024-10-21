using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.ContrAgents;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.ContrAgents;
using SmartMarket.Service.DTOs.Partners;
using SmartMarket.Service.DTOs.Tolov;
using SmartMarket.Service.Interfaces.ContrAgents;

namespace SmartMarket.Service.Services.ContrAgents;

public class ContrAgentService : IContrAgentService
{
    private readonly IMapper _mapper;
    private readonly IRepository<TolovUsuli> _tolovUsuliRepository;
    private readonly IRepository<ContrAgent> _agentRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Tolov> _tolovRepository;

    public ContrAgentService(
        IMapper mapper,
        IRepository<ContrAgent> agentRepository,
        IRepository<Product> productRepository,
        IRepository<Tolov> tolovRepository,
        IRepository<TolovUsuli> tolovUsuliRepository)
    {
        _mapper = mapper;
        _agentRepository = agentRepository;
        _productRepository = productRepository;
        _tolovRepository = tolovRepository;
        _tolovUsuliRepository = tolovUsuliRepository;
    }

    public async Task<ContrAgentForResultDto> CreateAsync(ContrAgentForCreationDto dto)
    {
        var agent = await _agentRepository.SelectAll()
           .Where(u => u.PhoneNumber == dto.PhoneNumber)
           .FirstOrDefaultAsync();

        if (agent is not null)
            throw new CustomException(403, "Bu agent mavjud.");

        var mapped = _mapper.Map<ContrAgent>(dto);
        mapped.TolovUsuliID = null;
        mapped.CreatedAt = DateTime.UtcNow;

        var result = await _agentRepository.InsertAsync(mapped);

        return _mapper.Map<ContrAgentForResultDto>(result);
    }

    public async Task<ContrAgentForResultDto> ModifyAsync(ContrAgentForUpdateDto dto)
    {
        var agent = await _agentRepository.SelectAll()
            .Where(u => u.Id == dto.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (agent is null)
            throw new CustomException(404, "Hamkor topilmadi.");

        var mapped = _mapper.Map(dto, agent);
        mapped.UpdatedAt = DateTime.UtcNow;

        await _agentRepository.UpdateAsync(mapped);

        return _mapper.Map<ContrAgentForResultDto>(mapped);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var agent = await _agentRepository.SelectAll()
              .Where(u => u.Id == id)
              .AsNoTracking()
              .FirstOrDefaultAsync();

        if (agent is null)
            throw new CustomException(404, "Agent topilmadi.");

        await _agentRepository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<ContrAgentForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var agents = await _agentRepository.SelectAll()
            .Include(u => u.Tolovs)
            .Include(p => p.Products)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ContrAgentForResultDto>>(agents);
    }

    public async Task<ContrAgentForResultDto> RetrieveByIdAsync(long id)
    {
        var agent = await _agentRepository.SelectAll()
             .Where(u => u.Id == id)
             .Include(u => u.Tolovs)
             .Include(p => p.Products)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (agent is null)
            throw new CustomException(404, "Agent topilmadi.");

        return _mapper.Map<ContrAgentForResultDto>(agent);
    }

    public async Task<ContrAgentForResultDto> PayForProductsAsync(long agentId, decimal paid, long tolovUsulID)
    {
        var agentProduct = await _productRepository.SelectAll()
            .Where(p => p.ContrAgentId == agentId)
            .FirstOrDefaultAsync();
        if (agentProduct is not null)
        {
            var partnerDebt = await _agentRepository.SelectAll()
            .Where(p => p.Id == agentProduct.ContrAgentId)
            .FirstOrDefaultAsync();
            if (partnerDebt.Dept > 0)
            {
                var nat = partnerDebt.Dept -= paid;
                partnerDebt.Dept = nat;
                partnerDebt.LastPaid = paid;
                partnerDebt.PayForDept += partnerDebt.LastPaid;
                partnerDebt.TolovUsuliID = tolovUsulID;
                partnerDebt.UpdatedAt = DateTime.UtcNow;

                var tolov = new Tolov
                {
                    ContrAgentId = partnerDebt.Id,
                    LastPaid = partnerDebt.LastPaid,
                    CreatedAt = DateTime.UtcNow
                };
                await _tolovRepository.InsertAsync(tolov);

                await _agentRepository.UpdateAsync(partnerDebt);

                if (partnerDebt.Dept == 0)
                {
                    partnerDebt.PayForDept = 0;
                    partnerDebt.LastPaid = 0;
                    partnerDebt.UpdatedAt = new DateTime(0000, 0, 0);
                }

                /*var tolovUsuli = await _tolovUsuliRepository.SelectAll()
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                tolovUsuli.Nasiya -= partnerDebt.LastPaid;
                await _tolovUsuliRepository.UpdateAsync(tolovUsuli);*/
            }
            else
            {
                throw new CustomException(400, "Qarz qolmadi.");
            }
        }
        return _mapper.Map<ContrAgentForResultDto>(agentProduct);
    }
}

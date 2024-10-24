﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.ContrAgents;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.Tolov;
using SmartMarket.Service.Interfaces.Partners;

namespace SmartMarket.Service.Services.Partners;

public class PartnerTolovService : IPartnerTolovService
{
    private readonly IRepository<PartnerTolov> _tolovRepository;
    private readonly IMapper _mapper;

    public PartnerTolovService(IRepository<PartnerTolov> tolovRepository, IMapper mapper)
    {
        _tolovRepository = tolovRepository;
        _mapper = mapper;
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var tolov = await _tolovRepository.SelectAll()
            .Where(x => x.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        if (tolov is null)
            throw new CustomException(404, "Tolov topilmadi.");

        await _tolovRepository.DeleteAsync(id);
        return true;
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TolovForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var tolovs = await _tolovRepository.SelectAll()
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TolovForResultDto>>(tolovs);
    }

    public async Task<TolovForResultDto> RetrieveByIdAsync(long id)
    {
        var tolov = await _tolovRepository.SelectAll()
             .Where(u => u.Id == id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (tolov is null)
            throw new CustomException(404, "Tolov topilmadi.");

        return _mapper.Map<TolovForResultDto>(tolov);
    }
}

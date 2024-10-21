using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.CencelOrders;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.CancelOrders;
using SmartMarket.Service.Interfaces.CencelOrders;

namespace SmartMarket.Service.Services.CencelOrders;

public class CancelOrderService : ICancelOrderService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Card> _cardRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<CencelOrder> _cencelOrdersRepository;
    private readonly IRepository<Partner> _partnerRepository;
    private readonly IRepository<PartnerProduct> _partnerProductRepository;

    public CancelOrderService(
        IRepository<CencelOrder> cencelOrdersRepository,
        IMapper mapper, IRepository<Card> cardRepository,
        IRepository<Product> productRepository,
        IRepository<PartnerProduct> partnerProductRepository, IRepository<Partner> partnerRepository)
    {
        _mapper = mapper;
        _cencelOrdersRepository = cencelOrdersRepository;
        _cardRepository = cardRepository;
        _productRepository = productRepository;
        _partnerProductRepository = partnerProductRepository;
        _partnerRepository = partnerRepository;
    }

    public async Task<CancelOrderForResultDto> CanceledProductsAsync(long id, decimal quantity, long canceledBy, string reason, bool action)
    {
        var card = await _cardRepository.SelectAll()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        if (card is not null && card.Quantity < quantity)
            throw new CustomException(404, $"Noto'g'ri son kirityabsiz, sotilgan yuklar soni: {card.Quantity}");


        var canceledOrder = new CencelOrder
        {
            TransNo = card.TransNo,
            ProductName = card.ProductName,
            BarCode = card.BarCode,
            CategoryId = card.CategoryId,
            Price = card.Price,
            OlchovTuri = card.OlchovBirligi,
            DiscountPrice = card.DiscountPrice,
            Quantity = quantity,
            CasherId = card.CasherId,
            Reason = reason,
            CancelerCasherId = canceledBy,
            ReturnDate = DateTime.UtcNow,
            Action = action,
            Status = "Kutilmoqda",
            CreatedAt = DateTime.UtcNow
        };

        canceledOrder.TotalPrice = canceledOrder.Price * canceledOrder.Quantity;

        await _cencelOrdersRepository.InsertAsync(canceledOrder);

        if (canceledOrder.Action)
        {
            var stok = await _productRepository.SelectAll()
                .Where(s => s.BarCode == canceledOrder.BarCode)
                .FirstOrDefaultAsync();

            if (stok is not null)
            {
                stok.Quantity += quantity;
                stok.UpdatedAt = DateTime.UtcNow;
                await _productRepository.UpdateAsync(stok);

                canceledOrder.Status = "Yaroqli";
                canceledOrder.UpdatedAt = DateTime.UtcNow;
                await _cencelOrdersRepository.UpdateAsync(canceledOrder);
            }
        }
        else
        {
            canceledOrder.Status = "Yaroqsiz";
            canceledOrder.UpdatedAt = DateTime.UtcNow;
            await _cencelOrdersRepository.UpdateAsync(canceledOrder);
        }


        return _mapper.Map<CancelOrderForResultDto>(canceledOrder);
    }

    public async Task<CancelOrderForResultDto> CanceledProductsFromParterAsync(long id, long partnerId, decimal quantity, long canceledBy, string reason, bool action)
    {
        var partnerProduct = await _partnerProductRepository.SelectAll()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        if (partnerProduct is not null && partnerProduct.Quantity < quantity)
            throw new CustomException(404, $"Noto'g'ri son kirityabsiz, sotilgan yuklar soni: {partnerProduct.Quantity}");


        var canceledOrder = new CencelOrder
        {
            TransNo = partnerProduct.TransNo,
            ProductName = partnerProduct.ProductName,
            BarCode = partnerProduct.BarCode,
            CategoryId = partnerProduct.CategoryId,
            Price = partnerProduct.Price,
            OlchovTuri = partnerProduct.OlchovBirligi,
            Quantity = quantity,
            CasherId = partnerProduct.UserId,
            DiscountPrice = partnerProduct.DiscountPrice,
            Reason = reason,
            CancelerCasherId = canceledBy,
            ReturnDate = DateTime.UtcNow,
            Action = action,
            Status = "Kutilmoqda",
            CreatedAt = DateTime.UtcNow
        };

        canceledOrder.TotalPrice = canceledOrder.Price * canceledOrder.Quantity;

        var partDebt = await _partnerRepository.SelectAll()
            .Where(p => p.Id == partnerId)
            .FirstOrDefaultAsync();
        if (partDebt.Debt != 0)
        {
            partDebt.Debt -= canceledOrder.TotalPrice;
            await _partnerRepository.UpdateAsync(partDebt);
        }

        await _cencelOrdersRepository.InsertAsync(canceledOrder);

        if (canceledOrder.Action)
        {
            var stok = await _productRepository.SelectAll()
                .Where(s => s.BarCode == canceledOrder.BarCode)
                .FirstOrDefaultAsync();

            if (stok is not null)
            {
                stok.Quantity += quantity;
                stok.UpdatedAt = DateTime.UtcNow;
                await _productRepository.UpdateAsync(stok);

                canceledOrder.Status = "Yaroqli";
                canceledOrder.UpdatedAt = DateTime.UtcNow;
                await _cencelOrdersRepository.UpdateAsync(canceledOrder);

                partnerProduct.Quantity -= quantity;
                partnerProduct.UpdatedAt = DateTime.UtcNow;
                await _partnerProductRepository.UpdateAsync(partnerProduct);
            }
        }
        else
        {
            canceledOrder.Status = "Yaroqsiz";
            canceledOrder.UpdatedAt = DateTime.UtcNow;
            await _cencelOrdersRepository.UpdateAsync(canceledOrder);

            partnerProduct.Quantity -= quantity;
            partnerProduct.UpdatedAt = DateTime.UtcNow;
            await _partnerProductRepository.UpdateAsync(partnerProduct);
        }


        return _mapper.Map<CancelOrderForResultDto>(partnerProduct);
    }

    public async Task<IEnumerable<CancelOrderForResultDto>> RetrieveAllWithDateTimeAsync(long userId, DateTime startDate, DateTime endDate)
    {
        if (userId != null)
        {
            var product = await _cencelOrdersRepository.SelectAll()
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate && p.CasherId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        var products = await _cencelOrdersRepository.SelectAll()
            .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<IEnumerable<CancelOrderForResultDto>>(products);
    }

    public async Task<IEnumerable<CancelOrderForResultDto>> YaroqsizMahsulotlarAsync(DateTime startDate, DateTime endDate)
    {
        var result = await _cencelOrdersRepository.SelectAll()
            .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
            .Where(c => c.Status == "Yaroqsiz")
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<IEnumerable<CancelOrderForResultDto>>(result);
    }

    public async Task<bool> ReamoveAsync(long id)
    {
        var order = await _cencelOrdersRepository.SelectAll()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        if (order is null)
            throw new CustomException(404, "Bekor qilingan buyurtma topilmadi.");

        await _cencelOrdersRepository.DeleteAsync(id);

        return true;
    }

    public async Task<IEnumerable<CancelOrderForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var orders = await _cencelOrdersRepository.SelectAll()
                .AsNoTracking()
                .ToPagedList(@params)
                .ToListAsync();

        return _mapper.Map<IEnumerable<CancelOrderForResultDto>>(orders);
    }

    public async Task<CancelOrderForResultDto> RetrieveByIdAsync(long id)
    {
        var order = await _cencelOrdersRepository.SelectAll()
                .Where(c => c.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        if (order is null)
            throw new CustomException(404, "Bekor qilingan buyurtma topilmadi.");

        return _mapper.Map<CancelOrderForResultDto>(order);
    }
}

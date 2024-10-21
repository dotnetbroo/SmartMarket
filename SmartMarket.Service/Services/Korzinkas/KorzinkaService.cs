using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Cards;
using SmartMarket.Domin.Entities.Orders;
using SmartMarket.Domin.Entities.Partners;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.Cards;
using SmartMarket.Service.DTOs.Korzinkas;
using SmartMarket.Service.Interfaces.Korzinkas;

namespace SmartMarket.Service.Services.Korzinkas;

public class KorzinkaService : IKorzinkaService
{
    private readonly IRepository<Partner> _partnerRepository;
    private readonly IRepository<TolovUsuli> _tolovRepository;
    private readonly IRepository<PartnerProduct> _partnerProductRepository;
    private readonly IRepository<Korzinka> _korzinkaRepository;
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Card> _cardRepository;
    private readonly IMapper _mapper;

    public KorzinkaService(
        IRepository<Korzinka> korzinkaRepository,
        IMapper mapper,
        IRepository<Order> orderRepository,
        IRepository<Product> productRepository,
        IRepository<Card> cardRepository,
        IRepository<PartnerProduct> partnerProductRepository,
        IRepository<Partner> partnerRepository,
        IRepository<TolovUsuli> tolovRepository)
    {
        _mapper = mapper;
        _korzinkaRepository = korzinkaRepository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _cardRepository = cardRepository;
        _partnerProductRepository = partnerProductRepository;
        _partnerRepository = partnerRepository;
        _tolovRepository = tolovRepository;
    }

    // Move products from orders to korzinka
    public async Task<KorzinkaForResultDto> MoveProductsFromOrderToKorzinkaAsync(string transNo)
    {
        var orders = await _orderRepository.SelectAll()
            .Where(o => o.TransNo == transNo)
            .ToListAsync();
        if (orders == null || !orders.Any())
            throw new CustomException(404, "Buyurtmalar topilmadi.");

        Korzinka lastInsertedKorzinka = null;
        foreach (var order in orders)
        {
            var korzinka = new Korzinka
            {
                TransNo = order.TransNo,
                ProductName = order.ProductName,
                BarCode = order.Barcode,
                CategoryId = order.CategoryId,
                PCode = order.PCode,
                Price = order.Price,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                YukYiguvchId = order.YiguvchId,
                YukTaxlovchId = order.YukTaxlovchId,
                PartnerId = order.PartnerId,
                OlchovBirligi = order.OlchovTuri,
                Status = "Kutilmoqda",
                CreatedAt = DateTime.UtcNow
            };

            lastInsertedKorzinka = await _korzinkaRepository.InsertAsync(korzinka);
        }

        foreach (var order in orders)
        {
            await _orderRepository.DeleteAsync(order.Id);
        }

        return _mapper.Map<KorzinkaForResultDto>(lastInsertedKorzinka);
    }

    public async Task<KorzinkaForResultDto> CalculeteDiscountPercentageAsync(long id, decimal discountPercentage)
    {
        var korzinka = await _korzinkaRepository.SelectAll()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();

        if (korzinka is null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        decimal discountAmount = (korzinka.TotalPrice / 100) * discountPercentage;

        decimal discountedTotalPrice = korzinka.TotalPrice - discountAmount;

        korzinka.TotalPrice = discountedTotalPrice;
        korzinka.DiscountPrice = discountAmount;

        await _korzinkaRepository.UpdateAsync(korzinka);

        return _mapper.Map<KorzinkaForResultDto>(korzinka);
    }

    public async Task<KorzinkaForResultDto> UpdateWithTransactionNumberAsync(string transactionNumber, long kassaId, long tolovUsuli, long sotuvchiId)
    {
        var korzinkas = await _korzinkaRepository.SelectAll()
            .Where(t => t.TransNo == transactionNumber)
            .ToListAsync();

        foreach (var korzinka in korzinkas)
        {
            if (korzinka != null)
            {
                korzinka.Status = "Sotildi";
                korzinka.TolovUsulId = tolovUsuli;
                korzinka.UpdatedAt = DateTime.UtcNow;
                await _korzinkaRepository.UpdateAsync(korzinka);
            }

            var card = new Card
            {
                TransNo = korzinka.TransNo,
                ProductName = korzinka.ProductName,
                BarCode = korzinka.BarCode,
                CategoryId = korzinka.CategoryId,
                Price = korzinka.Price,
                PCode = korzinka.PCode,
                DiscountPrice = korzinka.DiscountPrice,
                Quantity = korzinka.Quantity,
                TotalPrice = korzinka.TotalPrice,
                YukYiguvchId = korzinka.YukYiguvchId,
                YukTaxlovchId = korzinka.YukTaxlovchId,
                CasherId = sotuvchiId,
                PartnerId = korzinka.PartnerId,
                KassaId = kassaId,
                Status = korzinka.Status,
                OlchovBirligi = korzinka.OlchovBirligi,
                TolovUsulId = korzinka.TolovUsulId,
                CreatedAt = DateTime.UtcNow,
            };
            await _cardRepository.InsertAsync(card);
        }

        return _mapper.Map<KorzinkaForResultDto>(korzinkas);
    }

    public async Task<KorzinkaForResultDto> NasiyaSavdoAsync(string transactionNumber, long partnerId, long kassaId, long tolovUsuli, long sotuvchiId)
    {
        var korzinkas = await _korzinkaRepository.SelectAll()
            .Where(t => t.TransNo == transactionNumber)
            .ToListAsync();

        PartnerProduct partnerProduct = null;
        foreach (var korzinka in korzinkas)
        {
            if (korzinka != null)
            {
                korzinka.Status = "Sotildi";
                korzinka.TolovUsulId = tolovUsuli;
                korzinka.UpdatedAt = DateTime.UtcNow;
                await _korzinkaRepository.UpdateAsync(korzinka);
            }

            partnerProduct = new PartnerProduct
            {
                PartnerId = partnerId,
                CategoryId = korzinka.CategoryId,
                UserId = sotuvchiId,
                YukYiguvchId = korzinka.YukYiguvchId,
                YukTaxlovchId = korzinka.YukTaxlovchId,
                ProductName = korzinka.ProductName,
                TransNo = korzinka.TransNo,
                PCode = korzinka.PCode,
                BarCode = korzinka.BarCode,
                Quantity = korzinka.Quantity,
                Price = korzinka.Price,
                KassaId = kassaId,
                TotalPrice = korzinka.TotalPrice,
                DiscountPrice = korzinka.DiscountPrice,
                Status = korzinka.Status,
                OlchovBirligi = korzinka.OlchovBirligi,
                TolovUsuliId = korzinka.TolovUsulId,
                CreatedAt = DateTime.UtcNow,
            };
            await _partnerProductRepository.InsertAsync(partnerProduct);
        }

        var partnerDebt = await _partnerRepository.SelectAll()
            .Where(p => p.Id == partnerProduct.PartnerId)
            .FirstOrDefaultAsync();
        partnerDebt.Debt += partnerProduct.TotalPrice;
        partnerDebt.UpdatedAt = DateTime.UtcNow;
        await _partnerRepository.UpdateAsync(partnerDebt);

        var tolov = await _tolovRepository.SelectAll()
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
        tolov.KassaId = kassaId;
        tolov.Nasiya = partnerDebt.Debt;
        tolov.Status = "Tolanmagan";
        await _tolovRepository.InsertAsync(tolov);

        return _mapper.Map<KorzinkaForResultDto>(korzinkas);
    }


    public async Task<KorzinkaForResultDto> GetByBarCodeAsync(string barCode)
    {
        var code = await _korzinkaRepository.SelectAll()
            .Where(c => c.BarCode == barCode)
            .ToListAsync();
        if (code is null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        return _mapper.Map<KorzinkaForResultDto>(code);
    }


    // whith id
    public async Task<KorzinkaForResultDto> MoveProductToCardAsync(long id, long? yukYiguvchId, long? yukTaxlovchi, long? partnerId, decimal quantityToMove, string transNo)
    {
        var product = await _productRepository.SelectAll()
            .Where(q => q.Id == id)
            .FirstOrDefaultAsync();
        if (product == null)
            throw new CustomException(404, "Mahsulot topilmadi.");
        else if (product.Quantity == 0)
            throw new CustomException(404, "Hozirda bu mahsulotimiz tugagan.");
        if (product != null && product.Quantity < quantityToMove)
            throw new CustomException(400, $"Magazinda buncha mahsulot mavjud emas.\nHozirda {product.Quantity} ta mahsulot bor.");


        var korzinka = new Korzinka
        {
            TransNo = transNo,
            ProductName = product.Name,
            BarCode = product.BarCode,
            PCode = product.PCode,
            CategoryId = product.CategoryId,
            Price = product.SalePrice ?? 0,
            Quantity = quantityToMove,
            TotalPrice = (product.SalePrice ?? 0) * quantityToMove,
            YukYiguvchId = yukYiguvchId,
            YukTaxlovchId = yukTaxlovchi,
            OlchovBirligi = product.OlchovTuri,
            PartnerId = partnerId,
            Status = "Kutilmoqda",
            CreatedAt = DateTime.UtcNow
        };

        var existingCard = await _korzinkaRepository.SelectAll()
            .Where(p => p.TransNo == korzinka.TransNo && p.BarCode == korzinka.BarCode)
            .FirstOrDefaultAsync();

        if (existingCard != null)
        {
            existingCard.Quantity += quantityToMove;
            existingCard.TotalPrice = existingCard.Price * existingCard.Quantity;
            existingCard.UpdatedAt = DateTime.UtcNow;
            await _korzinkaRepository.UpdateAsync(existingCard);

            product.Quantity -= korzinka.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
        }
        else
        {
            await _korzinkaRepository.InsertAsync(korzinka);

            product.Quantity -= korzinka.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
        }

        return _mapper.Map<KorzinkaForResultDto>(korzinka);
    }

    // By Barcode
    public async Task<KorzinkaForResultDto> SaleProductWithBarCodeAsync(string barCode, long? yukYiguvchId, long? yukTaxlovchi, long? partnerId, decimal quantityToMove, string transNo)
    {
        var product = await _productRepository.SelectAll()
            .Where(q => q.BarCode == barCode)
            .FirstOrDefaultAsync();
        if (product == null)
            throw new CustomException(404, "Mahsulot topilmadi.");
        else if (product.Quantity == 0)
            throw new CustomException(404, "Hozirda bu mahsulotimiz tugagan.");
        if (product != null && product.Quantity < quantityToMove)
            throw new CustomException(400, $"Magazinda buncha mahsulot mavjud emas.\nHozirda {product.Quantity} ta mahsulot bor.");


        var korzinka = new Korzinka
        {
            TransNo = transNo,
            ProductName = product.Name,
            BarCode = product.BarCode,
            PCode = product.PCode,
            CategoryId = product.CategoryId,
            Price = product.SalePrice ?? 0,
            Quantity = quantityToMove,
            TotalPrice = (product.SalePrice ?? 0) * quantityToMove,
            YukYiguvchId = yukYiguvchId,
            YukTaxlovchId = yukTaxlovchi,
            OlchovBirligi = product.OlchovTuri,
            PartnerId = partnerId,
            Status = "Kutilmoqda",
            CreatedAt = DateTime.UtcNow
        };

        var existingCard = await _korzinkaRepository.SelectAll()
            .Where(p => p.TransNo == korzinka.TransNo && p.BarCode == korzinka.BarCode)
            .FirstOrDefaultAsync();

        if (existingCard != null)
        {
            existingCard.Quantity += quantityToMove;
            existingCard.TotalPrice = existingCard.Price * existingCard.Quantity;
            existingCard.UpdatedAt = DateTime.UtcNow;
            await _korzinkaRepository.UpdateAsync(existingCard);

            product.Quantity -= korzinka.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
        }
        else
        {
            await _korzinkaRepository.InsertAsync(korzinka);

            product.Quantity -= korzinka.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
        }

        return _mapper.Map<KorzinkaForResultDto>(korzinka);
    }

    public async Task<IEnumerable<KorzinkaForResultDto>> SvetUchgandaAsync(string status)
    {
        var result = await _cardRepository.SelectAll()
            .Where(c => c.Status.ToLower() == status.ToLower())
            .AsNoTracking()
            .ToListAsync();
        if (result is null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        return _mapper.Map<IEnumerable<KorzinkaForResultDto>>(result);
    }

    private static int lastTransactionNumberSuffix = 1000;
    private static DateTime lastTransactionDate = DateTime.UtcNow.Date;

    public string GenerateTransactionNumber()
    {
        DateTime currentDate = DateTime.UtcNow.Date;

        if (currentDate > lastTransactionDate)
        {
            lastTransactionNumberSuffix = 1001;
            lastTransactionDate = currentDate;
        }

        string transactionNumber;
        do
        {
            transactionNumber = currentDate.ToString("yyyyMMdd") + lastTransactionNumberSuffix.ToString();
            lastTransactionNumberSuffix++;
        } while (_korzinkaRepository.SelectAll().Any(t => t.TransNo == transactionNumber));

        return transactionNumber;
    }

    public async Task<bool> ReamoveAsync(long id)
    {
        var category = await _korzinkaRepository.SelectAll()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        if (category is null)
            throw new CustomException(404, "Korzinka topilmadi.");

        await _korzinkaRepository.DeleteAsync(id);

        return true;
    }

    public async Task<IEnumerable<KorzinkaForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var korzinkas = await _korzinkaRepository.SelectAll()
                .AsNoTracking()
                .ToPagedList(@params)
                .ToListAsync();

        return _mapper.Map<IEnumerable<KorzinkaForResultDto>>(korzinkas);
    }

    public async Task<KorzinkaForResultDto> RetrieveByIdAsync(long id)
    {
        var korzinka = await _korzinkaRepository.SelectAll()
                .Where(c => c.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        if (korzinka is null)
            throw new CustomException(404, "Korzinka topilmadi.");

        return _mapper.Map<KorzinkaForResultDto>(korzinka);
    }

    public async Task<bool> CanceledOrderByKorzinkaAsync(long id, string barCode, decimal quantity)
    {
        if (quantity < 0)
            throw new CustomException(400, "Soni 0 dan kichik bo'lmasligi kerak");

        var orderTask = _korzinkaRepository.SelectAll()
            .Where(t => t.Id == id)
            .FirstOrDefaultAsync();

        var productTask = _productRepository.SelectAll()
            .Where(i => i.BarCode.ToLower() == barCode.ToLower())
            .FirstOrDefaultAsync();

        var order = await orderTask;
        var product = await productTask;

        if (order is null)
            throw new CustomException(404, "Buyurtma topilmadi.");

        if (product is null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        product.Quantity += quantity;
        product.TotalPrice += (product.SalePrice ?? 0) * quantity;
        await _productRepository.UpdateAsync(product);

        await _korzinkaRepository.DeleteAsync(id);

        return true;
    }
}

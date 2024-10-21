using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.Orders;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.DTOs.Orders;
using SmartMarket.Service.Interfaces.Orders;

namespace SmartMarket.Service.Services.Orders;

public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<Product> _productRepository;

    public OrderService(IRepository<Order> orderRepository, IMapper mapper, IRepository<Product> productRepository)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _productRepository = productRepository;
    }


    public async Task<OrderForResultDto> MoveProductToOrderAsync(long id, long yukTaxlovchId, long yukYiguvchId, long partnerId, decimal quantityToMove, string transNo)
    {
        var product = await _productRepository.SelectAll()
            .Where(q => q.Id == id)
            .FirstOrDefaultAsync();

        if (product == null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        else if (product.Quantity == 0)
            throw new CustomException(404, "Hozirda bu mahsulotimiz tugagan.");

        if (product != null && product.Quantity < quantityToMove)
            throw new CustomException(400, $"Bazada buncha mahsulot mavjud emas.\nHozirda {product.Quantity} ta mahsulot bor.");

        var order = new Order
        {
            TransNo = transNo,
            ProductName = product.Name,
            Barcode = product.BarCode,
            PCode = product.PCode,
            CategoryId = product.CategoryId,
            Price = product.SalePrice ?? 0,
            YukTaxlovchId = yukTaxlovchId,
            YiguvchId = yukYiguvchId,
            PartnerId = partnerId,
            Quantity = quantityToMove,
            OlchovTuri = product.OlchovTuri,
            TotalPrice = (product.SalePrice ?? 0) * quantityToMove,
        };

        var existingOrder = await _orderRepository.SelectAll()
        .Where(p => p.TransNo == order.TransNo && p.Barcode == order.Barcode)
        .FirstOrDefaultAsync();

        if (existingOrder != null)
        {
            existingOrder.Quantity += quantityToMove;
            existingOrder.TotalPrice = existingOrder.Price * existingOrder.Quantity;
            existingOrder.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateAsync(existingOrder);

            product.Quantity -= order.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
        }
        else
        {
            await _orderRepository.InsertAsync(order);

            product.Quantity -= order.Quantity;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
        }

        return _mapper.Map<OrderForResultDto>(order);
    }


    public async Task<bool> CanceledOrderByPlanshetAsync(long id, string barCode, decimal quantity)
    {
        if (quantity < 0)
            throw new CustomException(400, "Soni 0 dan kichik bo'lmasligi kerak");

        var orderTask = _orderRepository.SelectAll()
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

        await _orderRepository.DeleteAsync(id);

        return true;
    }


    public async Task<bool> ReamoveAsync(long id)
    {
        var order = await _orderRepository.SelectAll()
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
        if (order is null)
            throw new CustomException(404, "Buyurtma topilmadi.");

        await _orderRepository.DeleteAsync(id);

        return true;
    }

    public async Task<IEnumerable<OrderForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var orders = await _orderRepository.SelectAll()
                .AsNoTracking()
                .ToPagedList(@params)
                .ToListAsync();

        return _mapper.Map<IEnumerable<OrderForResultDto>>(orders);
    }

    public async Task<OrderForResultDto> RetrieveByIdAsync(long id)
    {
        var order = await _orderRepository.SelectAll()
                .Where(c => c.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        if (order is null)
            throw new CustomException(404, "Buyurtma topilmadi.");

        return _mapper.Map<OrderForResultDto>(order);
    }

    public async Task<OrderForResultDto> ModifyAsync(long id, OrderForUpdateDto dto)
    {
        var order = await _orderRepository.SelectAll()
            .Where(o => o.Id == id)
            .FirstOrDefaultAsync();
        if (order is null)
            throw new CustomException(404, "Buyurtma topilmadi.");
        
        if(order.Quantity != dto.Quantity)
        {
            order.Quantity = dto.Quantity;
            order.TotalPrice = order.Price * order.Quantity;
        }

        var mappedOrder = _mapper.Map(dto, order);
        mappedOrder.UpdatedAt = DateTime.UtcNow;

        await _orderRepository.UpdateAsync(mappedOrder);

        return _mapper.Map<OrderForResultDto>(mappedOrder);
    }

    private static int lastTransactionNumberSuffix = 3000;
    private static DateTime lastTransactionDate = DateTime.UtcNow.Date;

    public string GenerateTransactionNumber()
    {
        DateTime currentDate = DateTime.UtcNow.Date;

        if (currentDate > lastTransactionDate)
        {
            lastTransactionNumberSuffix = 3001;
            lastTransactionDate = currentDate;
        }

        string transactionNumber;
        do
        {
            transactionNumber = currentDate.ToString("yyyyMMdd") + lastTransactionNumberSuffix.ToString();
            lastTransactionNumberSuffix++;
        } while (_orderRepository.SelectAll().Any(t => t.TransNo == transactionNumber));

        return transactionNumber;
    }

}

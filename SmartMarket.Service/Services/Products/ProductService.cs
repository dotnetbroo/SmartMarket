using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartMarket.Data.IRepositories;
using SmartMarket.Domin.Configurations;
using SmartMarket.Domin.Entities.ContrAgents;
using SmartMarket.Domin.Entities.Kassas;
using SmartMarket.Domin.Entities.Products;
using SmartMarket.Domin.Entities.Tolovs;
using SmartMarket.Service.Commons.Exceptions;
using SmartMarket.Service.Commons.Extensions;
using SmartMarket.Service.Commons.Helpers;
using SmartMarket.Service.DTOs.Products;
using SmartMarket.Service.Interfaces.Categories;
using SmartMarket.Service.Interfaces.Products;
using SmartMarket.Service.Interfaces.Users;

namespace SmartMarket.Service.Services.Products;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ICategoryService _categoryService;
    private readonly IRepository<TolovUsuli> _tolovRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<ContrAgent> _contrAgentRepository;
    private readonly IRepository<ProductStory> _productStoryRepository;

    public ProductService(
        IMapper mapper,
        ICategoryService categoryService,
        IRepository<Product> productsRepository,
        IUserService userService,
        IRepository<ContrAgent> contrAgentRepository,
        IRepository<TolovUsuli> tolovRepository,
        IRepository<ProductStory> productStoryRepository)
    {
        _productRepository = productsRepository;
        _mapper = mapper;
        _categoryService = categoryService;
        _userService = userService;
        _contrAgentRepository = contrAgentRepository;
        _tolovRepository = tolovRepository;
        _productStoryRepository = productStoryRepository;
    }

    public async Task<ProductForResultDto> CreateAsync(ProductForCreationDto productForCreationDto)
    {
        var product = await _productRepository.SelectAll()
            .Where(p => p.BarCode == productForCreationDto.BarCode)
            .FirstOrDefaultAsync();

        var categoryTask = _categoryService.RetrieveByIdAsync(productForCreationDto.CategoryId);
        var userTask = _userService.RetrieveByIdAsync(productForCreationDto.UserId);

        if (product != null)
        {
            await HandleExistingProductAsync(product, productForCreationDto);
            return _mapper.Map<ProductForResultDto>(product);
        }

        var fileName = await SaveProductImageAsync(productForCreationDto.ImagePath);
        var resultPath = Path.Combine("Media", "Products", fileName);

        var mappedProduct = _mapper.Map<Product>(productForCreationDto);
        mappedProduct.PCode = GeneratePCode();
        mappedProduct.TotalPrice = (productForCreationDto.SalePrice ?? 0) * productForCreationDto.Quantity;
        mappedProduct.ImagePath = resultPath;
        UpdatePriceAndPercentage(mappedProduct, productForCreationDto);
        mappedProduct.CreatedAt = DateTime.UtcNow;

        await UpdateAgentDeptAsync(mappedProduct.ContrAgentId, mappedProduct.TotalPrice);

        var productStory = CreateProductStory(mappedProduct);
        await _productStoryRepository.InsertAsync(productStory);

        var result = await _productRepository.InsertAsync(mappedProduct);
        return _mapper.Map<ProductForResultDto>(result);
    }

    private async Task HandleExistingProductAsync(Product product, ProductForCreationDto dto)
    {
        if (dto.Action)
        {
            product.CamePrice = (product.CamePrice != dto.CamePrice)
                ? (product.CamePrice + dto.CamePrice) / 2
                : product.CamePrice;
        }
        else
        {
            product.CamePrice = dto.CamePrice;
        }

        product.Quantity += dto.Quantity;
        UpdatePriceAndPercentage(product, dto);
        product.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product);

        product.TotalPrice = (product.SalePrice ?? 0) * product.Quantity;
        await _productRepository.UpdateAsync(product);

        await UpdateAgentDeptAsync(product.ContrAgentId, product.TotalPrice);
        throw new CustomException(200, "Bu turdagi mahsulot mavjudligi uchun uning soniga qo'shib qo'yildi.");
    }

    private async Task<string> SaveProductImageAsync(IFormFile image)
    {
        var wwwRootPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, "Media", "Products");
        var assetsFolderPath = Path.Combine(wwwRootPath, "Media");
        var videosFolderPath = Path.Combine(assetsFolderPath, "Products");

        Directory.CreateDirectory(assetsFolderPath);
        Directory.CreateDirectory(videosFolderPath);

        var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
        var fullPath = Path.Combine(wwwRootPath, fileName);

        await using (var stream = File.OpenWrite(fullPath))
        {
            await image.CopyToAsync(stream);
            await stream.FlushAsync();
        }

        return fileName;
    }

    private async Task UpdateAgentDeptAsync(long contrAgentId, decimal totalPrice)
    {
        var agentDept = await _contrAgentRepository.SelectAll()
            .Where(a => a.Id == contrAgentId)
            .FirstOrDefaultAsync();

        agentDept.Dept += totalPrice;
        agentDept.UpdatedAt = DateTime.UtcNow;

        await _contrAgentRepository.UpdateAsync(agentDept);
    }

    private ProductStory CreateProductStory(Product product)
    {
        return new ProductStory
        {
            PCode = product.PCode,
            BarCode = product.BarCode,
            Name = product.Name,
            CategoryId = product.CategoryId,
            UserId = product.UserId,
            CamePrice = product.CamePrice,
            Quantity = product.Quantity,
            TotalPrice = product.TotalPrice,
            OlchovTuri = product.OlchovTuri,
            SalePrice = product.SalePrice,
            PercentageOfPrice = product.PercentageOfPrice,
            Action = product.Action,
            ImagePath = product.ImagePath,
            ContrAgentId = product.ContrAgentId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdatePriceAndPercentage(Product product, ProductForCreationDto dto)
    {
        if (dto.SalePrice == null && dto.PercentageOfPrice != null)
        {
            decimal newPrice = (product.CamePrice / 100) * dto.PercentageOfPrice.Value;
            product.SalePrice = product.CamePrice + newPrice;
            product.PercentageOfPrice = dto.PercentageOfPrice.Value;
        }
        else if (dto.SalePrice != null && dto.PercentageOfPrice == null)
        {
            decimal percentPrice = ((dto.SalePrice.Value - product.CamePrice) / product.CamePrice) * 100;
            product.PercentageOfPrice = percentPrice;
            product.SalePrice = dto.SalePrice;
        }
    }



    public async Task<bool> DeleteAsync(long id)
    {
        var product = await _productRepository.SelectAll()
            .Where(p => p.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (product is null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        var fullPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, product.ImagePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return await _productRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ProductForResultDto>> GetAllAsync(PaginationParams @params)
    {
        var products = await _productRepository.SelectAll()
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ProductForResultDto>>(products);
    }

    public async Task<ProductForResultDto> GetByIdAsync(long id)
    {
        var product = await _productRepository.SelectAll()
             .Where(p => p.Id == id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (product is null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        return _mapper.Map<ProductForResultDto>(product);
    }

    public async Task<ProductForResultDto> UpdateAsync(ProductForUpdateDto productForUpdateDto)
    {
        var product = await _productRepository.SelectAll()
            .Where(p => p.Id == productForUpdateDto.Id)
            .FirstOrDefaultAsync();

        if (product is null)
            throw new CustomException(404, "Mahsulot topilmadi.");

        var category = await _categoryService.RetrieveByIdAsync(productForUpdateDto.CategoryId);
        var user = await _userService.RetrieveByIdAsync(productForUpdateDto.UserId);

        if (product.SalePrice is null && product.PercentageOfPrice is not null)
        {
            decimal? newPrice = (product.CamePrice / 100) * product.PercentageOfPrice;
            decimal? salePrice = product.CamePrice + newPrice;
            product.SalePrice = salePrice;
        }
        else if (product.SalePrice is not null && product.PercentageOfPrice is null)
        {
            decimal? percentPrice = ((product.CamePrice - product.SalePrice) / product.CamePrice) * 100;
            decimal? percentSale = 100 - percentPrice;
            product.PercentageOfPrice = percentSale;
        }


        var fullPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, product.ImagePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(productForUpdateDto.ImagePath.FileName);
        var rootPath = Path.Combine(WebHostEnviromentHelper.WebRootPath, "Media", "Products", fileName);
        using (var stream = new FileStream(rootPath, FileMode.Create))
        {
            await productForUpdateDto.ImagePath.CopyToAsync(stream);
            await stream.FlushAsync();
            stream.Close();
        }
        string resultImage = Path.Combine("Media", "Products", fileName);

        var mapped = _mapper.Map(productForUpdateDto, product);
        mapped.UpdatedAt = DateTime.UtcNow;
        mapped.ImagePath = resultImage;

        await _productRepository.UpdateAsync(mapped);

        return _mapper.Map<ProductForResultDto>(mapped);
    }

    public string GeneratePCode()
    {
        var random = new Random();

        string pCode;
        do
        {
            int randomSuffix = random.Next(1000, 9999);
            pCode = "P" + randomSuffix.ToString();
        } while (_productRepository.SelectAll().Any(t => t.PCode == pCode));

        return pCode;
    }

}
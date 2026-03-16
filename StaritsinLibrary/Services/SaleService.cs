using Microsoft.EntityFrameworkCore;
using StaritsinLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary.Services
{
    public class SaleService
    {
        private readonly StaritsinDbContextFactory _factory;
        private readonly DiscountService _discountService;

        public SaleService(StaritsinDbContextFactory factory, DiscountService discountService)
        {
            _factory = factory;
            _discountService = discountService;
        }

        public async Task<List<SaleListItemDTO>> GetSalesAsync()
        {
            using (var context = _factory.CreateContext())
            {
                return await context.PartnerSales
                    .AsNoTracking()
                    .Include(x => x.Partner)
                    .Include(x => x.Product)
                    .OrderByDescending(x => x.SaleDate)
                    .ThenByDescending(x => x.Id)
                    .Select(x => new SaleListItemDTO
                    {
                        Id = x.Id,
                        PartnerId = x.PartnerId,
                        PartnerName = x.Partner != null ? x.Partner.CompanyName : string.Empty,
                        ProductId = x.ProductId,
                        ProductName = x.Product != null ? x.Product.ProductName : string.Empty,
                        Quantity = x.Quantity,
                        SaleDate = x.SaleDate,
                        BasePrice = x.BasePrice,
                        DiscountPercent = x.DiscountPercent,
                        UnitPrice = x.UnitPrice,
                        TotalAmount = x.TotalAmount,
                        Comment = x.Comment
                    })
                    .ToListAsync();
            }
        }

        public async Task<SaleEditModel> GetSaleByIdAsync(int id)
        {
            using (var context = _factory.CreateContext())
            {
                return await context.PartnerSales
                    .AsNoTracking()
                    .Where(x => x.Id == id)
                    .Select(x => new SaleEditModel
                    {
                        Id = x.Id,
                        PartnerId = x.PartnerId,
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        SaleDate = x.SaleDate,
                        Comment = x.Comment
                    })
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<List<SalePartnerLookupDTO>> GetPartnersForLookupAsync()
        {
            using (var context = _factory.CreateContext())
            {
                return await context.Partners
                    .AsNoTracking()
                    .OrderBy(x => x.CompanyName)
                    .Select(x => new SalePartnerLookupDTO
                    {
                        Id = x.Id,
                        CompanyName = x.CompanyName
                    })
                    .ToListAsync();
            }
        }

        public async Task<List<SaleProductLookupDTO>> GetProductsForLookupAsync()
        {
            using (var context = _factory.CreateContext())
            {
                return await context.Products
                    .AsNoTracking()
                    .OrderBy(x => x.ProductName)
                    .Select(x => new SaleProductLookupDTO
                    {
                        Id = x.Id,
                        ProductName = x.ProductName,
                        Price = x.Price
                    })
                    .ToListAsync();
            }
        }

        public async Task<SaleCalculationResultDTO> CalculateSaleAsync(
            int partnerId, int productId, int quantity, int? editingSaleId = null)
        {
            if (partnerId <= 0)
                throw new ArgumentException("Не выбран партнёр.");

            if (productId <= 0)
                throw new ArgumentException("Не выбран продукт.");

            if (quantity <= 0)
                throw new ArgumentException("Количество должно быть больше нуля.");

            using (var context = _factory.CreateContext())
            {
                var product = await context.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == productId);

                if (product == null)
                    throw new InvalidOperationException("Продукт не найден.");

                var totalQuantity = await context.PartnerSales
                    .AsNoTracking()
                    .Where(x => x.PartnerId == partnerId &&
                                (!editingSaleId.HasValue || x.Id != editingSaleId.Value))
                    .SumAsync(x => (int?)x.Quantity) ?? 0;

                int discountPercent = _discountService.CalculateDiscount(totalQuantity);

                decimal unitPrice = Math.Round(product.Price * (1m - discountPercent / 100m), 2,
                                                  MidpointRounding.AwayFromZero);
                decimal totalAmount = Math.Round(unitPrice * quantity, 2,
                                                  MidpointRounding.AwayFromZero);

                return new SaleCalculationResultDTO
                {
                    BasePrice = product.Price,
                    DiscountPercent = discountPercent,
                    UnitPrice = unitPrice,
                    TotalAmount = totalAmount
                };
            }
        }
        public async Task<int> AddSaleAsync(SaleEditModel model)
        {
            Validate(model);

            using (var context = _factory.CreateContext())
            {
                var calculation = await CalculateSaleAsync(model.PartnerId, model.ProductId, model.Quantity);

                var entity = new PartnerSale
                {
                    PartnerId = model.PartnerId,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity,
                    SaleDate = model.SaleDate,
                    BasePrice = calculation.BasePrice,
                    DiscountPercent = calculation.DiscountPercent,
                    UnitPrice = calculation.UnitPrice,
                    TotalAmount = calculation.TotalAmount,
                    Comment = NormalizeNullable(model.Comment)
                };

                context.PartnerSales.Add(entity);
                await context.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task UpdateSaleAsync(SaleEditModel model)
        {
            Validate(model);

            using (var context = _factory.CreateContext())
            {
                var entity = await context.PartnerSales.FirstOrDefaultAsync(x => x.Id == model.Id);

                if (entity == null)
                    throw new InvalidOperationException("Продажа не найдена.");

                var calculation = await CalculateSaleAsync(model.PartnerId, model.ProductId, model.Quantity, model.Id);

                entity.PartnerId = model.PartnerId;
                entity.ProductId = model.ProductId;
                entity.Quantity = model.Quantity;
                entity.SaleDate = model.SaleDate;
                entity.BasePrice = calculation.BasePrice;
                entity.DiscountPercent = calculation.DiscountPercent;
                entity.UnitPrice = calculation.UnitPrice;
                entity.TotalAmount = calculation.TotalAmount;
                entity.Comment = NormalizeNullable(model.Comment);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteSaleAsync(int id)
        {
            using (var context = _factory.CreateContext())
            {
                var entity = await context.PartnerSales.FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                    throw new InvalidOperationException("Продажа не найдена.");

                context.PartnerSales.Remove(entity);
                await context.SaveChangesAsync();
            }
        }


        private static void Validate(SaleEditModel model)
        {
            if (model.PartnerId <= 0)
                throw new ArgumentException("Не выбран партнёр.");

            if (model.ProductId <= 0)
                throw new ArgumentException("Не выбран продукт.");

            if (model.Quantity <= 0)
                throw new ArgumentException("Количество должно быть больше нуля.");

            if (model.SaleDate == default(DateTime))
                throw new ArgumentException("Укажите дату продажи.");
        }

        private static string NormalizeNullable(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}

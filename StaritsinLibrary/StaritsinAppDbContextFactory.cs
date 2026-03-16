using Microsoft.EntityFrameworkCore;
using StaritsinLibrary.Models;
using System;
using System.Collections.Generic;

namespace StaritsinLibrary
{

    public class StaritsinDbContextFactory
    {
        private readonly string _connectionString;

        public StaritsinDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public StaritsinDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<StaritsinDbContext>()
                .UseNpgsql(_connectionString)
                .Options;

            return new StaritsinDbContext(options);
        }

        public bool EnsureCreated()
        {
            using (var context = CreateContext())
            {
                context.Database.ExecuteSqlRaw("CREATE SCHEMA IF NOT EXISTS app");
            }

            bool wasCreated;
            using (var context = CreateContext())
            {
                wasCreated = context.Database.EnsureCreated();
            }

            if (wasCreated)
            {
                using (var context = CreateContext())
                {
                    SeedInitialData(context);
                }
            }

            return wasCreated;
        }

        private static void SeedInitialData(StaritsinDbContext context)
        {
            var partnerTypes = new List<PartnerType>
            {
                new PartnerType { TypeName = "ЗАО" },
                new PartnerType { TypeName = "ООО" },
                new PartnerType { TypeName = "ПАО" },
                new PartnerType { TypeName = "ОАО" }
            };
            context.PartnerTypes.AddRange(partnerTypes);
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product { ProductName = "Паркет тип 1",     Unit = "м²", Price = 1200.00m },
                new Product { ProductName = "Паркет тип 2",     Unit = "м²", Price = 1500.00m },
                new Product { ProductName = "Ламинат стандарт", Unit = "м²", Price =  800.00m },
                new Product { ProductName = "Ламинат премиум",  Unit = "м²", Price = 1100.00m }
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            var partners = new List<Partner>
            {
                new Partner
                {
                    PartnerTypeId = partnerTypes[1].Id,  
                    CompanyName   = "СтройМастер",
                    DirectorName  = "Иванов Иван Иванович",
                    Email         = "info@stroymaster.ru",
                    Phone         = "+7 495 123 45 67",
                    Address       = "г. Москва, ул. Строителей, д. 1",
                    Rating        = 7
                },
                new Partner
                {
                    PartnerTypeId = partnerTypes[0].Id, 
                    CompanyName   = "ПаркетПлюс",
                    DirectorName  = "Петров Пётр Петрович",
                    Email         = "info@parketplus.ru",
                    Phone         = "+7 812 234 56 78",
                    Address       = "г. Санкт-Петербург, пр. Невский, 10",
                    Rating        = 5
                },
                new Partner
                {
                    PartnerTypeId = partnerTypes[2].Id,  
                    CompanyName   = "ДомДекор",
                    DirectorName  = "Сидоров Сидор Сидорович",
                    Email         = "info@domdecor.ru",
                    Phone         = "+7 343 345 67 89",
                    Address       = "г. Екатеринбург, ул. Декораторов, 3",
                    Rating        = 9
                }
            };
            context.Partners.AddRange(partners);
            context.SaveChanges();


            var sales = new List<PartnerSale>
            {
                MakeSale(partners[0].Id, products[0].Id,
                    quantity: 15000, date: new DateTime(2024, 3, 15),
                    basePrice: 1200.00m, discountPercent: 0),

                MakeSale(partners[0].Id, products[2].Id,
                    quantity: 8000, date: new DateTime(2024, 7, 20),
                    basePrice: 800.00m, discountPercent: 5),
 
                MakeSale(partners[1].Id, products[1].Id,
                    quantity: 62000, date: new DateTime(2024, 1, 10),
                    basePrice: 1500.00m, discountPercent: 0),
 
                MakeSale(partners[2].Id, products[3].Id,
                    quantity: 3000, date: new DateTime(2024, 11, 5),
                    basePrice: 1100.00m, discountPercent: 0),
 
                MakeSale(partners[2].Id, products[0].Id,
                    quantity: 45000, date: new DateTime(2025, 2, 18),
                    basePrice: 1200.00m, discountPercent: 0)
            };
            context.PartnerSales.AddRange(sales);
            context.SaveChanges();
        }


        private static PartnerSale MakeSale(
            int partnerId, int productId,
            int quantity, DateTime date,
            decimal basePrice, int discountPercent)
        {
            decimal unitPrice = Math.Round(
                basePrice * (1m - discountPercent / 100m), 2,
                MidpointRounding.AwayFromZero);

            decimal totalAmount = Math.Round(
                unitPrice * quantity, 2,
                MidpointRounding.AwayFromZero);

            return new PartnerSale
            {
                PartnerId = partnerId,
                ProductId = productId,
                Quantity = quantity,
                SaleDate = date,
                BasePrice = basePrice,
                DiscountPercent = discountPercent,
                UnitPrice = unitPrice,
                TotalAmount = totalAmount,
                Comment = null
            };
        }
    }
}

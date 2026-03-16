using Microsoft.EntityFrameworkCore;
using StaritsinLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                bool wasCreated = context.Database.EnsureCreated();

                if (wasCreated)
                {
                    SeedInitialData(context);
                }

                return wasCreated;
            }
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
                new Product { ProductName = "Паркет тип 1",     Unit = "м²" },
                new Product { ProductName = "Паркет тип 2",     Unit = "м²" },
                new Product { ProductName = "Ламинат стандарт", Unit = "м²" },
                new Product { ProductName = "Ламинат премиум",  Unit = "м²" }
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
                new PartnerSale
                {
                    PartnerId = partners[0].Id,
                    ProductId = products[0].Id,
                    Quantity  = 15000,
                    SaleDate  = new System.DateTime(2024, 3, 15)
                },
                new PartnerSale
                {
                    PartnerId = partners[0].Id,
                    ProductId = products[2].Id,
                    Quantity  = 8000,
                    SaleDate  = new System.DateTime(2024, 7, 20)
                },
                new PartnerSale
                {
                    PartnerId = partners[1].Id,
                    ProductId = products[1].Id,
                    Quantity  = 62000,
                    SaleDate  = new System.DateTime(2024, 1, 10)
                },
                new PartnerSale
                {
                    PartnerId = partners[2].Id,
                    ProductId = products[3].Id,
                    Quantity  = 3000,
                    SaleDate  = new System.DateTime(2024, 11, 5)
                },
                new PartnerSale
                {
                    PartnerId = partners[2].Id,
                    ProductId = products[0].Id,
                    Quantity  = 45000,
                    SaleDate  = new System.DateTime(2025, 2, 18)
                }
            };
            context.PartnerSales.AddRange(sales);
            context.SaveChanges();
        }
    }
}

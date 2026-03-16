using Microsoft.EntityFrameworkCore;
using StaritsinLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaritsinLibrary.Repositories
{
    public class PartnerRepository
    {
        private readonly StaritsinDbContextFactory _factory;

        public PartnerRepository(StaritsinDbContextFactory factory)
        {
            _factory = factory;
        }


        public async Task<List<Partner>> GetAllAsync()
        {
            using (var context = _factory.CreateContext())
            {
                return await context.Partners
                    .Include(p => p.PartnerType)
                    .OrderBy(p => p.CompanyName)
                    .ToListAsync();
            }
        }

        public async Task<Partner> GetByIdAsync(int id)
        {
            using (var context = _factory.CreateContext())
            {
                return await context.Partners
                    .Include(p => p.PartnerType)
                    .Include(p => p.Sales)
                        .ThenInclude(s => s.Product)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
        }

        public async Task AddAsync(Partner partner)
        {
            using (var context = _factory.CreateContext())
            {
                context.Partners.Add(partner);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Partner partner)
        {
            using (var context = _factory.CreateContext())
            {
                context.Partners.Update(partner);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var context = _factory.CreateContext())
            {
                var partner = await context.Partners.FindAsync(id);
                if (partner != null)
                {
                    context.Partners.Remove(partner);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<PartnerType>> GetPartnerTypesAsync()
        {
            using (var context = _factory.CreateContext())
            {
                return await context.PartnerTypes
                    .OrderBy(t => t.TypeName)
                    .ToListAsync();
            }
        }

        public async Task<List<PartnerSale>> GetSalesByPartnerAsync(int partnerId)
        {
            using (var context = _factory.CreateContext())
            {
                return await context.PartnerSales
                    .Include(s => s.Product)
                    .Where(s => s.PartnerId == partnerId)
                    .OrderByDescending(s => s.SaleDate)
                    .ToListAsync();
            }
        }

        public async Task<int> GetTotalQuantityByPartnerAsync(int partnerId)
        {
            using (var context = _factory.CreateContext())
            {
                var sum = await context.PartnerSales
                    .Where(s => s.PartnerId == partnerId)
                    .SumAsync(s => (int?)s.Quantity);

                return sum ?? 0;
            }
        }
    }
}

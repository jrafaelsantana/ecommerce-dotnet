using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext _dbContext;
        private readonly ILogger<CustomersProvider> _logger;
        private readonly IMapper _mapper;

        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._logger = logger;
            this._mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!_dbContext.Customers.Any())
            {
                _dbContext.Customers.Add(new Db.Customer() {Id = 1, Name = "Rafael", Address = "Rua ABC"});
                _dbContext.Customers.Add(new Db.Customer() {Id = 2, Name = "José", Address = "Rua DEF"});
                _dbContext.Customers.Add(new Db.Customer() {Id = 3, Name = "Higor", Address = "Rua GHI"});
                _dbContext.Customers.Add(new Db.Customer() {Id = 4, Name = "Pedro", Address = "Rua JKL"});
                _dbContext.Customers.Add(new Db.Customer() {Id = 5, Name = "Lucas", Address = "Rua MNO"});

                _dbContext.SaveChanges();
            }
        }
        
        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var customers = await _dbContext.Customers.ToListAsync();

                if (customers != null && customers.Any())
                {
                    var result = _mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await _dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);

                if (customer != null)
                {
                    var result = _mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result, null);
                }

                return (false, null, "Not found");
            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
using ETickets.SharedLibrary.Implementaion;
using ETickets.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : GenericClass<Order>(context), IOrder
    {
        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                return await context.Orders.Where(predicate).ToListAsync();
            }
            catch (Exception ex) 
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while placing order");
            }
        }
    }
}

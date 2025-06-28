using OrderApi.Domain.Entities;

namespace OrderApi.Application.DTOs.Conversions
{
    public class OrderConversion
    {
        public static Order ToEntity(OrderDTO orderDTO) => new Order()
        {
            Id= orderDTO.Id,
            ClientId = orderDTO.ClientId,
            MovieId= orderDTO.MovieId,  
            OrderedDate = orderDTO.OrderedDate, 
            //PurchaseSeats = orderDTO.PurchaseSeats
        };

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            if (order != null) 
            {
                var singleOrder = new OrderDTO(
                order.Id,
                order.MovieId,
                order.ClientId,
                //order.PurchaseSeats,
                order.OrderedDate
                );
                return (singleOrder, null);
            }

            if (orders != null)
            {
                var _orders = orders.Select(o=> 
                new OrderDTO(
                    o.Id,
                    o.MovieId,
                    o.ClientId,
                    //o.PurchaseSeats,
                    o.OrderedDate)
                );
                return (null, _orders);
            }
            return (null, null);    
        }

    }
}

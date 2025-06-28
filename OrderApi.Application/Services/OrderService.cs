using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;
using System.Net.Http.Json;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {

        // Get Movie
        public async Task<MovieDTO> GetMovie(int movieId)
        {
            var getMovie = await httpClient.GetAsync($"api/movie/{movieId}");
            if (!getMovie.IsSuccessStatusCode) return null!;
            var movie = await getMovie.Content.ReadFromJsonAsync<MovieDTO>();
            return movie!;
        }
        
        // Get User
        public async Task<AppUserDTO> GetUser(int userId)
        {
            var getUser = await httpClient.GetAsync($"api/authentication/{userId}");
            if (!getUser.IsSuccessStatusCode) return null!;
            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;
        }

        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            var order = await orderInterface.FindByIdAsync(orderId);
            if(order == null || order.Id  == 0) return null!;
            var retryPipeLine = resiliencePipeline.GetPipeline("my-retry-pipeline");
            var movieDTO = await retryPipeLine.ExecuteAsync(async token => await GetMovie(order.MovieId));
            var appUserDTO = await retryPipeLine.ExecuteAsync(async token => await GetUser(order.ClientId));
            return new OrderDetailsDTO(
                order.Id,
                movieDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Email,
                appUserDTO.Address,
                appUserDTO.TelephoneNumber,
                movieDTO.Name,
                order.PurchaseSeats,
                movieDTO.Price,
                movieDTO.Price,
                order.OrderedDate
                );
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if (!orders.Any()) return null!;

            var (_, _orders) = OrderConversion.FromEntity(null, orders);
            return _orders;
        }
    }
}

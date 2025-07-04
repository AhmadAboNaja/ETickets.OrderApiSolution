﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using OrderApi.Application.Services;

namespace OrderApi.Presentaion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if(!orders.Any()) return NotFound("No order detected in the database");

            var (_, list) = OrderConversion.FromEntity(null, orders);
            return !list!.Any() ? NotFound("") : Ok(list);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await orderInterface.FindByIdAsync(id);
            if (order == null) return NotFound("Order Not Found");
            var (_order, _) = OrderConversion.FromEntity(order, null);
            return Ok(_order);
        }

        [HttpGet("client/{clientId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderByClientId(int clientId)
        {
            if(clientId <= 0) return NotFound("Invalid Data Provided");
            var orders = await orderService.GetOrdersByClientId(clientId);
            return !orders.Any()? NotFound(null) : Ok(orders);
        }

        [HttpGet("details/{orderId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            if (orderId <= 0) return NotFound("Invalid Data Provided");
            var orderDetails = await orderService.GetOrderDetails(orderId);
            return orderDetails.OrderId > 0 ? Ok(orderDetails) : NotFound("No order found");
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid) return BadRequest("Incomplete data submitted");
            var getEntity = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(getEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(OrderDTO orderDTO)
        {
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteOrder(OrderDTO orderDTO)
        {
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.DeleteAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}
